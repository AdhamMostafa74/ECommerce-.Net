
using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Application.Common.Payments;

public interface IPaymentService
{
    Task<PaymentInitiationResult> CreatePaymentAsync(
        Order order,
        string customerEmail,
        CancellationToken ct = default);
}
