using ECommerce.Application.Types.DTOs;

namespace ECommerce.Application.Types;

public interface ITypeQueryService
{
    Task<IReadOnlyList<GetAllTypesResponse>> GetAllTypeResponse(
        CancellationToken ct = default);

    Task<GetByIdTypeResponse?> GetByIdTypeResponse(
        Guid id,
        CancellationToken ct = default);
}