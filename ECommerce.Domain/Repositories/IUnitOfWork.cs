using ECommerce.Domain.Entities.BasketEntities;

namespace ECommerce.Domain.Repositories;

public interface IUnitOfWork
{
    IRepository<T> Repository<T>() where T : BaseEntity;
    Task<int> SaveChangeAsync(CancellationToken ct = default );
}
