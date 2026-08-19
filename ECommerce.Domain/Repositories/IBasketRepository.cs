using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IBasketRepository
{
    Task<Basket?> GetAsync(
        Guid basketId,
        CancellationToken ct = default);

    Task SaveAsync(
        Basket basket,
        CancellationToken ct = default);

    Task DeleteAsync(
        Guid basketId,
        CancellationToken ct = default);
}