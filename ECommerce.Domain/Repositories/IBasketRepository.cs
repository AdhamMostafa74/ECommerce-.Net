using ECommerce.Domain.Entities.Basket;

namespace ECommerce.Domain.Repositories;

public interface IBasketRepository
{
    Task<BasketEntity?> GetAsync(
        Guid userId,
        CancellationToken ct = default);

    Task SaveAsync(
        Guid userId,
        BasketEntity basket,
        CancellationToken ct = default);

    Task DeleteAsync(
        Guid userId,
        CancellationToken ct = default);
}