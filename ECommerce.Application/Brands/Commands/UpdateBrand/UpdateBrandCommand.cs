using ECommerce.Application.Brands.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Brands.Commands.UpdateBrand;

public sealed record UpdateBrandCommand(
    Guid Id,
    string Name
) : IRequest<Result<BrandResponse>>;