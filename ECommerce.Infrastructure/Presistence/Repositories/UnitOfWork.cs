using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Data.DbContexts;
using System.Collections.Concurrent;

namespace ECommerce.Infrastructure.Presistence.Repositories;

public class UnitOfWork(ECommerceDbContext db) : IUnitOfWork
{
    private readonly ConcurrentDictionary<Type, object> _repo = new();
    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);
        if (_repo.TryGetValue(type, out var repo))
            return (IRepository<T>)repo;

        var newRepo = new Repository<T>(db);
        _repo.TryAdd(type, newRepo);
        return newRepo;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await db.SaveChangesAsync(ct);
    }
}

