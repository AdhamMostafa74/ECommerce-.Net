using ECommerce.Domain.Entities.Basket;

namespace ECommerce.Domain.Repositories;

public interface IBasketRepository
{
    Task<BasketEntity?> GetAsync(
        Guid basketId,
        CancellationToken ct = default);

    Task SaveAsync(
        BasketEntity basket,
        CancellationToken ct = default);

    Task DeleteAsync(
        Guid basketId,
        CancellationToken ct = default);
}