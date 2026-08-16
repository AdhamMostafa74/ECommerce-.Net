using ECommerce.Application.Types.DTOs;
using ECommerce.Application.Types.Errors;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Types.Queries;

public sealed class GetByIdTypeQuery(
    ITypeQueryService typeQueryService)
{
    public async Task<Result<GetByIdTypeResponse>> ExecuteAsync(Guid id)
    {
        var type = await typeQueryService.GetByIdTypeResponse(id);


        return type;

    }
}