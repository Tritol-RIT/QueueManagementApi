using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Core;

namespace QueueManagementApi.Infrastructure.Interceptors;

internal class AuditableInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        // Logic before saving changes
        OnBeforeSaveChanges(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        // Async logic before saving changes
        OnBeforeSaveChanges(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void OnBeforeSaveChanges(DbContext context)
    {
        // Set IAuditable fields on EntityState.Added and EntityState.Modified
        var entries = context.ChangeTracker.Entries();
        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditable entity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // Set properties if entity is added
                        entity.CreatedOn = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        // Set properties if entity is modified
                        entity.UpdatedOn = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
