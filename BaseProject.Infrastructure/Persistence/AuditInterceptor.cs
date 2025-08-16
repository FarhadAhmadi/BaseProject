using BaseProject.Application.Common.Interfaces;
using BaseProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace BaseProject.Infrastructure.Persistence
{
    public class AuditInterceptor(ICurrentUser currentUser) : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;

            if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            var auditEntries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .Select(e => CreateAuditEntry(e))
                .ToList();

            context.Set<AuditLog>().AddRange(auditEntries);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private AuditLog CreateAuditEntry(EntityEntry entry)
        {
            var userId = currentUser.GetCurrentUserId();

            var changes = new Dictionary<string, object?>();

            switch (entry.State)
            {
                case EntityState.Added:
                    foreach (var prop in entry.CurrentValues.Properties)
                    {
                        changes[prop.Name] = new
                        {
                            OldValue = (string?)null,
                            NewValue = entry.CurrentValues[prop.Name]
                        };
                    }
                    break;

                case EntityState.Modified:
                    foreach (var prop in entry.OriginalValues.Properties)
                    {
                        var original = entry.OriginalValues[prop.Name];
                        var current = entry.CurrentValues[prop.Name];

                        if (!Equals(original, current))
                        {
                            changes[prop.Name] = new
                            {
                                OldValue = original,
                                NewValue = current
                            };
                        }
                    }
                    break;

                case EntityState.Deleted:
                    foreach (var prop in entry.OriginalValues.Properties)
                    {
                        changes[prop.Name] = new
                        {
                            OldValue = entry.OriginalValues[prop.Name],
                            NewValue = (string?)null
                        };
                    }
                    break;
            }

            return new AuditLog
            {
                EntityName = entry.Entity.GetType().Name,
                ActionType = entry.State.ToString(),
                Changes = JsonSerializer.Serialize(changes, new JsonSerializerOptions
                {
                    WriteIndented = false,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                Timestamp = DateTime.UtcNow,
                UserId = userId
            };
        }
    }
}
