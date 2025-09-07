using BaseProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.Configurations.Identity;

public class RefreshTokenConfig : IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.ToTable("RefreshTokens", "Identity");

        //builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token)
               .HasColumnType("nvarchar(max)")
               .IsRequired();

        builder.Property(rt => rt.Created)
               .IsRequired();

        builder.Property(rt => rt.Expires)
               .IsRequired();

        builder.HasOne(rt => rt.User)
               .WithMany(u => u.RefreshTokens)
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}