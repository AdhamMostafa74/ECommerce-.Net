using ECommerce.Application.Basket.Errors;
using ECommerce.Application.Common.Identity;
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
    ICurrentUser currentUser)
    : IRequestHandler<CreateOrderCommand, Result<Guid>>
    {
    public async Task<Result<Guid>> Handle(
        CreateOrderCommand request,
        CancellationToken ct)
        {
        var basket = await basketRepository.GetAsync(
            currentUser.UserId,
            ct);

        if (basket is null || basket.Items.Count == 0)
            {
            return Result<Guid>.Failure(
                OrderErrors.EmptyBasket);
            }

        var productIds = basket.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToArray();

        if (productIds.Any(id => id == Guid.Empty))
            {
            return Result<Guid>.Failure(
                OrderErrors.InvalidBasket);
            }

        var productRepository =
            unitOfWork.Repository<Product>();

        var products = await productRepository.ListAsync(
            new ProductsForOrderCreationSpecification(productIds),
            ct);

        if (products.Count != productIds.Length)
            {
            return Result<Guid>.Failure(
                ProductErrors.NotFound);
            }

        var productsById = products.ToDictionary(
            product => product.Id);

        foreach (var basketItem in basket.Items)
            {
            if (basketItem.Quantity <= 0 ||
                !productsById.ContainsKey(basketItem.ProductId))
                {
                return Result<Guid>.Failure(
                    OrderErrors.InvalidBasket);
                }
            }

        var customerRepository =
            unitOfWork.Repository<Customer>();

        var customer = await customerRepository.FirstOrDefaultAsync(
            new CustomerByApplicationUserIdSpecification(
                currentUser.UserId),
            ct);

        if (customer is null || customer.IsDeleted)
            {
            return Result<Guid>.Failure(
                OrderErrors.CustomerUnavailable);
            }

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
            shippingAddress);

        foreach (var basketItem in basket.Items)
            {
            var product =
                productsById[basketItem.ProductId];

            order.AddItem(
                product.Id,
                product.Name,
                product.PictureUrl,
                product.Price,
                basketItem.Quantity);
            }

        var orderRepository =
            unitOfWork.Repository<Order>();

        orderRepository.Create(order);

        await unitOfWork.SaveChangesAsync(ct);

        await basketRepository.DeleteAsync(
            currentUser.UserId,
            ct);

        return Result<Guid>.Success(order.Id);
        }
    }