using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerce.Infrastructure.Presistence.Interceptors
{
    public class Interceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            UpdateAuditFields(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateAuditFields(eventData.Context);

            return base.SavingChangesAsync(
                eventData,
                result,
                cancellationToken);
        }

        private static void UpdateAuditFields(DbContext? context)
        {
            if (context == null)
                return;

            var entries = context.ChangeTracker
                .Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:

                        entry.Entity.MarkCreated(Environment.UserName);
                        break;

                    case EntityState.Modified:

                        entry.Entity.MarkUpdated(Environment.UserName);

                        break;

                    case EntityState.Deleted:

                        entry.Entity.MarkAsDeleted(Environment.UserName);

                        break;
                }
            }
        }
    }
}