using BaseProject.Domain.Entities.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.Configurations.Authorization
{
    public class RolePermissionActionConfiguration : IEntityTypeConfiguration<RolePermissionAction>
    {
        public void Configure(EntityTypeBuilder<RolePermissionAction> builder)
        {
            builder.ToTable("RolePermissionActions", "Authorization");
        }
    }
}
