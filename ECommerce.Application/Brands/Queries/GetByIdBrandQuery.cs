using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Brands.Errors;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Brands.Queries;

public sealed class GetByIdBrandQuery(
    IBrandQueryService brandQueryService)
{
    public async Task<Result<GetByIdBrandResponse>> ExecuteAsync(Guid id)
    {
        var brand = await brandQueryService.GetByIdBrandResponse(id);

        if (brand != null)
            return Result<GetByIdBrandResponse>.Success(brand);

        return Result<GetByIdBrandResponse>.Failure(BrandErrors.NotFound);
    }
}