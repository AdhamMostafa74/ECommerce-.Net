
using ECommerce.Application.Basket.Errors;
using ECommerce.Application.Common.Identity;
using ECommerce.Application.Common.Payments;
using ECommerce.Application.Orders.Errors;
using ECommerce.Application.Products.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.CustomersSpecifications;
using ECommerce.Domain.Common.Specifications.ProductsSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Orders.Commands.CreateOrder;

public sealed class CreateOrderHandler(
    IUnitOfWork unitOfWork,
    IBasketRepository basketRepository,
    ICurrentUser currentUser,
    IIdentityService identityService,
    IPaymentService paymentService)
    : IRequestHandler<
        CreateOrderCommand,
        Result<CreateOrderResponse>>
{
    public async Task<Result<CreateOrderResponse>> Handle(
        CreateOrderCommand request,
        CancellationToken ct)
    {
        var basket = await basketRepository.GetAsync(
            currentUser.UserId,
            ct);

        if (basket is null || basket.Items.Count == 0)
            return Result<CreateOrderResponse>.Failure(
                OrderErrors.EmptyBasket);

        var productIds = basket.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToArray();

        if (productIds.Any(id => id == Guid.Empty))
            return Result<CreateOrderResponse>.Failure(
                OrderErrors.InvalidBasket);

        var productRepository =
            unitOfWork.Repository<Product>();

        var products = await productRepository.ListAsync(
            new ProductsForOrderCreationSpecification(productIds),
            ct);

        if (products.Count != productIds.Length)
            return Result<CreateOrderResponse>.Failure(
                ProductErrors.NotFound);

        var productsById = products.ToDictionary(
            product => product.Id);

        foreach (var basketItem in basket.Items)
        {
            if (basketItem.Quantity <= 0 ||
                !productsById.ContainsKey(basketItem.ProductId))
            {
                return Result<CreateOrderResponse>.Failure(
                    OrderErrors.InvalidBasket);
            }
        }

        string? customerEmail = null;

        if (request.PaymentMethod == PaymentMethod.Online)
        {
            customerEmail = await identityService.GetEmailAsync(
                currentUser.UserId,
                ct);

            if (string.IsNullOrWhiteSpace(customerEmail))
            {
                // Do not create an order that cannot be paid.
                return Result<CreateOrderResponse>.Failure(
                    OrderErrors.CustomerUnavailable);
            }
        }

        var customerRepository =
            unitOfWork.Repository<Customer>();

        var customer = await customerRepository.FirstOrDefaultAsync(
            new CustomerByApplicationUserIdSpecification(
                currentUser.UserId),
            ct);

        if (customer is null || customer.IsDeleted)
            return Result<CreateOrderResponse>.Failure(
                OrderErrors.CustomerUnavailable);

        var shippingAddress = new Address(
            customer.FirstName,
            customer.LastName,
            customer.PhoneNumber,
            request.Street,
            request.City,
            request.State,
            request.PostalCode,
            request.Country);

        var order = Order.Create(
            customer.Id,
            shippingAddress,
            request.PaymentMethod);

        foreach (var basketItem in basket.Items)
        {
            var product = productsById[basketItem.ProductId];

            order.AddItem(
                product.Id,
                product.Name,
                product.PictureUrl,
                product.Price,
                basketItem.Quantity);
        }

        var orderRepository = unitOfWork.Repository<Order>();

        orderRepository.Create(order);

        // Save first so the order has a stable ID for Paymob.
        await unitOfWork.SaveChangesAsync(ct);

        string? paymentClientSecret = null;

        if (request.PaymentMethod == PaymentMethod.Online)
        {
            // If this fails, the basket remains available for retry.
            var paymentResult = await paymentService.CreatePaymentAsync(
                order,
                customerEmail!,
                ct);

            var payment = Payment.Create(
                order.Id,
                "Paymob",
                paymentResult.PaymentIntentId,
                order.TotalPrice,
                paymentResult.Currency);

            order.AddPayment(payment);

            paymentClientSecret = paymentResult.ClientSecret;

            await unitOfWork.SaveChangesAsync(ct);
        }

        // Delete only after order/payment initiation succeeds.
        await basketRepository.DeleteAsync(
            currentUser.UserId,
            ct);

        return Result<CreateOrderResponse>.Success(
            new CreateOrderResponse(
                order.Id,
                paymentClientSecret));
    }
}
