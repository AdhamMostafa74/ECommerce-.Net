using ECommerce.Application.Types.DTOs;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Types.Queries;

public sealed class GetAllTypesQuery(
    ITypeQueryService typeQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllTypesResponse>>> ExecuteAsync()
    {
        var types = await typeQueryService.GetAllTypeResponse();

        return Result<IReadOnlyList<GetAllTypesResponse>>
            .Success(types);
    }
}