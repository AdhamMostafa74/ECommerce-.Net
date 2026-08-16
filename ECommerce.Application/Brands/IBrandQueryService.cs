using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Brands
{
    public interface IBrandQueryService
    {
        Task<Result<PaginatedResult<GetAllBrandsResponse>>> GetAllBrandResponse(
                PaginationRequest pagination,

            CancellationToken ct = default);

        Task<Result<GetByIdBrandResponse>> GetByIdBrandResponse(
            Guid id,
            CancellationToken ct = default);
    }
}