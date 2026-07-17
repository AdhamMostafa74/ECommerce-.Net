
using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Common.Results;
using System.Threading;

namespace ECommerce.Application.Products;

public interface IProductQueryService
{
    Task<IReadOnlyList<GetAllProductResponse>> GetAllProductResponse(CancellationToken ct = default);
    Task<GetByIdProductResponse?> GetByIdProductResponse(Guid id ,CancellationToken ct = default);
}
