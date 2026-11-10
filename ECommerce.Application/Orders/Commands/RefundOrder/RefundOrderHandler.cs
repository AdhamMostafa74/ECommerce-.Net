using ECommerce.Application.Common.Payments;
using ECommerce.Application.Orders.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.OrdersSpecifications;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Orders.Commands.RefundOrder;

public sealed class RefundOrderHandler(
    IUnitOfWork unitOfWork,
    IPaymentService paymentService,
    ILogger<RefundOrderHandler> logger)
    : IRequestHandler<RefundOrderCommand, Result<RefundOrderResponse>>
{
    public async Task<Result<RefundOrderResponse>> Handle(
        RefundOrderCommand request,
        CancellationToken ct)
    {
        var orderRepo = unitOfWork.Repository<Order>();
        var order = await orderRepo.FirstOrDefaultAsync(
            new OrderWithPaymentsByIdSpecification(request.OrderId),
            ct);

        if (order is null)
            return Result<RefundOrderResponse>.Failure(OrderErrors.NotFound);

        if (order.PaymentStatus != PaymentStatus.Paid || order.PaymentMethod != PaymentMethod.Online)
            return Result<RefundOrderResponse>.Failure(OrderErrors.NotEligibleForRefund);

        var payment = order.Payments.FirstOrDefault(p =>
            p.Status == PaymentStatus.Paid &&
            !string.IsNullOrWhiteSpace(p.ProviderTransactionId));

        if (payment is null)
            return Result<RefundOrderResponse>.Failure(OrderErrors.NotEligibleForRefund);

        PaymentRefundResult refundResult;
        try
        {
            refundResult = await paymentService.RefundPaymentAsync(
                payment,
                request.Amount,
                ct);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to refund order {OrderId} with Paymob for transaction {TransactionId}.",
                order.Id,
                payment.ProviderTransactionId);

            return Result<RefundOrderResponse>.Failure(OrderErrors.RefundFailed);
        }

        payment.MarkAsRefunded();
        order.MarkPaymentAsRefunded();

        if (order.Status == OrderStatus.Pending || order.Status == OrderStatus.Processing)
        {
            order.Cancel();
        }

        await unitOfWork.SaveChangesAsync(ct);

        return Result<RefundOrderResponse>.Success(
            new RefundOrderResponse(
                order.Id,
                refundResult.RefundTransactionId,
                refundResult.Amount,
                order.PaymentStatus,
                order.Status));
    }
}

