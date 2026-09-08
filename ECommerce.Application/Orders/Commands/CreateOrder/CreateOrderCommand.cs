
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country) : IRequest<Result<Guid>>;
