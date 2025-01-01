using DomainAbstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Database.Interceptors;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ChangeDeletedStateToUpdated(eventData.Context!);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        ChangeDeletedStateToUpdated(eventData.Context!);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ChangeDeletedStateToUpdated(DbContext dbContext)
    {
        foreach (var entry in dbContext.ChangeTracker.Entries())
        {
            if (entry is { Entity: ISoftDeletable entity, State: EntityState.Deleted })
            {
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }
    }
}
