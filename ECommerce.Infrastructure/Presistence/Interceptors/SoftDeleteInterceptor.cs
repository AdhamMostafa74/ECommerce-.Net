using ECommerce.Domain.Entities.BasketEntities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Presistence.Interceptors;

public class SoftDeleteInterceptor : ISoftDeleteInterceptor
{
    public void Apply(DbContext db)
    {
        foreach (var entry in db.ChangeTracker.Entries<BaseEntity>())
        {

            if (entry.State != EntityState.Deleted)
                continue;

            entry.State = EntityState.Modified;
            entry.Entity.MarkAsDeleted(Environment.UserName);
        }

    }


}
