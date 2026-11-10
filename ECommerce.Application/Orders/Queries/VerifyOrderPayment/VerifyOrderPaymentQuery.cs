using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Orders.Queries.VerifyOrderPayment;

public sealed record VerifyOrderPaymentQuery(
    Guid OrderId)
    : IRequest<Result<VerifyOrderPaymentResponse>>;

