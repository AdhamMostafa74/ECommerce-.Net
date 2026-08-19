using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Presistence.Specification;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Presistence.Repositories;

public class Repository<T>(ECommerceDbContext context) : IRepository<T>
    where T : BaseEntity
{
    private readonly ECommerceDbContext _context = context;

    public async Task<IReadOnlyList<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken ct)
    {

        return await ApplySpecification(specification).ToListAsync(ct);
    }

    public async Task<T?> FirstOrDefaultAsync(
        ISpecification<T> specification,
        CancellationToken ct)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(ct);


    }

    public async Task<int> CountAsync(
        ISpecification<T> specification,
        CancellationToken ct)
    {



        return await ApplySpecification(specification).AsNoTracking().CountAsync(ct);
    }

    public async Task<bool> AnyAsync(
        ISpecification<T> specification,
        CancellationToken ct)
    {
        return await ApplySpecification(specification).AsNoTracking().AnyAsync(ct);

    }

    public void Create(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator.GetQuery(
            _context.Set<T>(),
            specification);
    }



}