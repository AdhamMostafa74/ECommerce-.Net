using ECommerce.Application.Brands.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Brands.Commands.CreateBrand;

public sealed record CreateBrandCommand(
    string Name
) : IRequest<Result<BrandResponse>>;