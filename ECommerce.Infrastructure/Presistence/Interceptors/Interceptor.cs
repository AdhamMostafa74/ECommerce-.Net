using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerce.Infrastructure.Presistence.Interceptors;

public class Interceptor(ICurrentUser currentUser) : SaveChangesInterceptor
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

    private void UpdateAuditFields(DbContext? context)
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
                    entry.Entity.MarkCreated(
                        currentUser.UserId.ToString());
                    break;

                case EntityState.Modified:
                    entry.Entity.MarkUpdated(
                        currentUser.UserId.ToString());
                    break;

                case EntityState.Deleted:
                    entry.Entity.MarkAsDeleted(
                        currentUser.UserId.ToString());
                    break;
            }
        }
    }
}