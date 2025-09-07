using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.Configurations.Identity;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users", "Identity");

        builder.Property(u => u.FullName)
               .HasMaxLength(256);

        builder.HasOne(u => u.Avatar)
               .WithOne(m => m.User)
               .HasForeignKey<ApplicationUser>(u => u.AvatarId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.RefreshTokens)
               .WithOne(rt => rt.User)
               .HasForeignKey(rt => rt.UserId)
               .IsRequired();

        // Each User can have many UserClaims
        builder.HasMany(e => e.Claims)
            .WithOne(e => e.User)
            .HasForeignKey(uc => uc.UserId)
            .IsRequired();

        // Each User can have many UserLogins
        builder.HasMany(e => e.Logins)
            .WithOne(e => e.User)
            .HasForeignKey(ul => ul.UserId)
            .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany(e => e.Tokens)
            .WithOne(e => e.User)
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();

        // Each User can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
            .WithOne(e => e.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
    }
}