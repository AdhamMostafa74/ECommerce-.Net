using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Presistence.Interceptors;

public class SoftDeleteInterceptor(
    ICurrentUser currentUser) : ISoftDeleteInterceptor
{
    public void Apply(DbContext db)
    {
        foreach (var entry in db.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State != EntityState.Deleted)
                continue;

            entry.State = EntityState.Modified;

            entry.Entity.MarkAsDeleted(
                currentUser.UserId.ToString());
        }
    }
}