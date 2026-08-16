
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id): IRequest<Result>;

