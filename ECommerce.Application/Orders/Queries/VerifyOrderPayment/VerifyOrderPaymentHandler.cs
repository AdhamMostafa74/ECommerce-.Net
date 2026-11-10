using ECommerce.Application.Common.Identity;
using ECommerce.Application.Orders.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.CustomersSpecifications;
using ECommerce.Domain.Common.Specifications.OrdersSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Orders.Queries.VerifyOrderPayment;

public sealed class VerifyOrderPaymentHandler(
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser)
    : IRequestHandler<VerifyOrderPaymentQuery, Result<VerifyOrderPaymentResponse>>
{
    public async Task<Result<VerifyOrderPaymentResponse>> Handle(
        VerifyOrderPaymentQuery request,
        CancellationToken ct)
    {
        var customerRepo = unitOfWork.Repository<Customer>();
        var customer = await customerRepo.FirstOrDefaultAsync(
            new CustomerByApplicationUserIdSpecification(currentUser.UserId),
            ct);

        if (customer is null || customer.IsDeleted)
            return Result<VerifyOrderPaymentResponse>.Failure(OrderErrors.CustomerUnavailable);

        var orderRepo = unitOfWork.Repository<Order>();
        var order = await orderRepo.FirstOrDefaultAsync(
            new OrderByIdAndCustomerSpecification(request.OrderId, customer.Id),
            ct);

        if (order is null)
            return Result<VerifyOrderPaymentResponse>.Failure(OrderErrors.NotFound);

        return Result<VerifyOrderPaymentResponse>.Success(
            new VerifyOrderPaymentResponse(
                order.Id,
                order.Status,
                order.PaymentStatus,
                order.PaymentStatus == PaymentStatus.Paid,
                order.TotalPrice,
                order.CreatedAt));
    }
}

