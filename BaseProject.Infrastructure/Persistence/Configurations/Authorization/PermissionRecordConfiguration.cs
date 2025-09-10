using BaseProject.Domain.Entities.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.Configurations.Authorization
{
    public class PermissionRecordConfiguration : IEntityTypeConfiguration<PermissionRecord>
    {
        public void Configure(EntityTypeBuilder<PermissionRecord> builder)
        {
            builder.ToTable("PermissionRecords", "Authorization");

            builder.HasMany(pr => pr.Actions)
                   .WithOne(pa => pa.PermissionRecord)
                   .HasForeignKey(pa => pa.PermissionRecordId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
