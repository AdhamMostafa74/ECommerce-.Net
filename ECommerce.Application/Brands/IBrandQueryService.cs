using ECommerce.Application.Brands.DTOs;

namespace ECommerce.Application.Brands
{
    public interface IBrandQueryService
    {
        Task<IReadOnlyList<GetAllBrandsResponse>> GetAllBrandResponse(
            CancellationToken ct = default);

        Task<GetByIdBrandResponse?> GetByIdBrandResponse(
            Guid id,
            CancellationToken ct = default);
    }
}