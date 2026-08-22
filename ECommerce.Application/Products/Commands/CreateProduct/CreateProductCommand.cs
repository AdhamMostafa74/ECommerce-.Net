

using ECommerce.Application.Common.Files;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string ProductName,
    string ProductDescription,
    decimal Price,
    FileUpload? Picture,
    Guid BrandId,
    Guid TypeId
    ) : IRequest<Result<Guid>>;

