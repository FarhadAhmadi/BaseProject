using BaseProject.Domain.Entities;
using BaseProject.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("ApplicationUser");

        builder.Property(x => x.Status).HasDefaultValue(Status.Active);

        builder.HasOne(x => x.Avatar).WithOne(x => x.User).HasForeignKey<ApplicationUser>(x => x.AvatarId);
    }
}
