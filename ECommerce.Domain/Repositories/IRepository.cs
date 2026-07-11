

using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IRepository<T> where T : BaseEntity
{

    Task<IEnumerable<T>> GetAll(CancellationToken ct);
    Task<T?> GetById(Guid Id ,CancellationToken ct);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);


}

