using BaseProject.Domain.Entities.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.Configurations.Authorization
{
    public class UserPermissionActionConfiguration : IEntityTypeConfiguration<UserPermissionAction>
    {
        public void Configure(EntityTypeBuilder<UserPermissionAction> builder)
        {
            builder.ToTable("UserPermissionActions", "Authorization");
        }
    }
}
