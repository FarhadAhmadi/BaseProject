using BaseProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.Configurations.Identity;

public class ForgotPasswordConfig : IEntityTypeConfiguration<UserPasswordReset>
{
    public void Configure(EntityTypeBuilder<UserPasswordReset> builder)
    {
        builder.ToTable("ForgotPasswords", "Identity");

        //builder.HasKey(f => f.Id);

        builder.Property(f => f.Email)
               .HasMaxLength(256)
               .IsRequired();

        builder.Property(f => f.Token)
               .HasMaxLength(512)
               .IsRequired();

        builder.Property(f => f.OTP)
               .HasMaxLength(10);

        builder.Property(f => f.DateTime)
               .IsRequired();

        //builder.HasOne(f => f.User)
        //       .WithMany(u => u.RefreshTokens) // if you want to link ForgotPassword, you can create a separate collection in ApplicationUser
        //       .HasForeignKey(f => f.UserId)
        //       .OnDelete(DeleteBehavior.Cascade);
    }
}
