using ECommerce.Application.Brands.DTOs;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Brands.Queries;

public sealed class GetAllBrandsQuery(
    IBrandQueryService brandQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllBrandsResponse>>> ExecuteAsync()
    {
        var brands = await brandQueryService.GetAllBrandResponse();

        return Result<IReadOnlyList<GetAllBrandsResponse>>
            .Success(brands);
    }
}