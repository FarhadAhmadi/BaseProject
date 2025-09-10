using BaseProject.Domain.Entities.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.Configurations.Authorization
{
    public class PermissionActionConfiguration : IEntityTypeConfiguration<PermissionAction>
    {
        public void Configure(EntityTypeBuilder<PermissionAction> builder)
        {
            builder.ToTable("PermissionActions", "Authorization");

            builder.HasMany(pa => pa.RolePermissionActions)
                   .WithOne(rpa => rpa.PermissionAction)
                   .HasForeignKey(rpa => rpa.PermissionActionId)
                   .OnDelete(DeleteBehavior.SetNull); // Set FK to NULL instead of cascade delete

            builder.HasMany(pa => pa.UserPermissionActions)
                   .WithOne(uo => uo.PermissionAction)
                   .HasForeignKey(uo => uo.PermissionActionId)
                   .OnDelete(DeleteBehavior.SetNull); // Set FK to NULL instead of cascade delete
        }
    }
}
