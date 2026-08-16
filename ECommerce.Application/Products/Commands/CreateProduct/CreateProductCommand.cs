

using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string ProductName,
    string ProductDescription,
    decimal Price,
    string PictureUrl,
    Guid BrandId,
    Guid TypeId
    ) : IRequest<Result<Guid>>;

