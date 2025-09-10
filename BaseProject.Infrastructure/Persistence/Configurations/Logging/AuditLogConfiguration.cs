using BaseProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Infrastructure.Persistence.Configurations.Logging
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            // Map to table "AuditLogs" in schema "Logging"
            builder.ToTable("AuditLogs", "Logging");

            // Configure primary key (inherited from BaseEntity, assumed Id)
            builder.HasKey(al => al.Id);

            // Configure properties
            builder.Property(al => al.EntityName)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(al => al.ActionType)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(al => al.Changes)
                   .HasColumnType("nvarchar(max)");

            builder.Property(al => al.UserId)
                   .HasMaxLength(50);

            builder.Property(al => al.Timestamp)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()"); // default to current UTC time

            // If you later add navigation properties, you can configure them like PermissionAction
            // e.g., builder.HasMany(...).WithOne(...).HasForeignKey(...).OnDelete(...)
        }
    }
}
