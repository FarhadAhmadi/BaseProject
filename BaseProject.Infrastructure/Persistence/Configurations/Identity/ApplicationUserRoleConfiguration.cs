using BaseProject.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.Configurations.Identity;

public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.ToTable("UserRoles", "Identity");

        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        builder.HasOne(ur => ur.User)
               .WithMany(u => u.UserRoles)
               .HasForeignKey(ur => ur.UserId);

        builder.HasOne(ur => ur.Role)
               .WithMany(r => r.UserRoles)
               .HasForeignKey(ur => ur.RoleId);
    }
}
