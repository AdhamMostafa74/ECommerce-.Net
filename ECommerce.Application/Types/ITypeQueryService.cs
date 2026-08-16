using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Types.DTOs;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Types;

public interface ITypeQueryService
{
    Task<Result<PaginatedResult<GetAllTypesResponse>>> GetAllTypeResponse(
        PaginationRequest paginationRequest,
        CancellationToken ct = default);

    Task<Result<GetByIdTypeResponse>> GetByIdTypeResponse(
        Guid id,
        CancellationToken ct = default);
}