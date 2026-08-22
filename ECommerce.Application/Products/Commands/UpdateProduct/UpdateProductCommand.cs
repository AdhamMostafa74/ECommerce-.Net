using ECommerce.Application.Common.Files;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string? Name,
    string? Description,
    FileUpload? Picture,
    decimal? Price,
    Guid? BrandId,
    Guid? TypeId
) : IRequest<Result>;