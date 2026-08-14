using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Commands.UpdateProduct
{
    public sealed record UpdateProductCommand(
        Guid Id,
        string? Name,
        string? Description,
        string? PictureUrl,
        decimal? Price,
        Guid? BrandId,
        Guid? TypeId
    ) : IRequest<Result>;
}