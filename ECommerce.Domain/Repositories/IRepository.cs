using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    void Create(T entity);
    void Update(T entity);

    Task<IReadOnlyList<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken ct);

    Task<T?> FirstOrDefaultAsync(
        ISpecification<T> specification,
        CancellationToken ct);

    Task<int> CountAsync(
        ISpecification<T> specification,
        CancellationToken ct);

    Task<bool> AnyAsync(
    ISpecification<T> specification,
    CancellationToken ct);

    IQueryable<T> ApplySpecification(ISpecification<T> specification);
}