using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Orders.Queries.GetOrderDetails;

public sealed record GetOrderDetailsQuery(
    Guid OrderId)
    : IRequest<Result<GetOrderDetailsResponse>>;