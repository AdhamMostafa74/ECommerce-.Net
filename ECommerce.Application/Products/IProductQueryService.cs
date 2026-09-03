using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Products;

public interface IProductQueryService
{
    Task<Result<PaginatedResult<GetAllProductResponse>>>
        GetAllProductResponse(
            PaginationRequest pagination,
            CancellationToken ct = default);

    Task<Result<PaginatedResult<GetAllProductResponse>>>
        GetDeletedProductResponse(
            PaginationRequest pagination,
            CancellationToken ct = default);

    Task<Result<PaginatedResult<GetAllProductResponse>>>
        GetAllProductsIncludingDeletedResponse(
            PaginationRequest pagination,
            CancellationToken ct = default);

    Task<Result<GetByIdProductResponse>>
        GetByIdProductResponse(
            Guid id,
            CancellationToken ct = default);
}