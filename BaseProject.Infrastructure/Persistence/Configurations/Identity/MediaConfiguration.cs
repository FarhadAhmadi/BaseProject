using BaseProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.Configurations.Identity;

public class MediaConfiguration : IEntityTypeConfiguration<UserMedia>
{
    public void Configure(EntityTypeBuilder<UserMedia> builder)
    {
        builder.ToTable("Media", "Identity");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.PathMedia)
               .HasMaxLength(1024)
               .IsRequired();

        builder.Property(m => m.Caption)
               .HasMaxLength(256);

        builder.Property(m => m.FileSize)
               .IsRequired();

        builder.Property(m => m.DateCreated)
               .IsRequired();
    }
}
