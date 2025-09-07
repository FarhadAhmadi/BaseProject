using BaseProject.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="GenericAttribute"/> entity.
    /// <para>
    /// This class defines how <see cref="GenericAttribute"/> is mapped to the database,
    /// including table name, primary key, property constraints, relationships, indexes, 
    /// and comments for documentation.
    /// </para>
    /// <para>
    /// Purpose:
    /// - Ensures the database schema matches the entity model.
    /// - Defines constraints and indexes for efficient querying.
    /// - Explicitly maps the one-to-many relationship with <see cref="BaseEntity"/>.
    /// </para>
    /// </summary>
    public class GenericAttributeConfiguration : IEntityTypeConfiguration<GenericAttribute>
    {
        /// <summary>
        /// Configures the <see cref="GenericAttribute"/> entity for EF Core.
        /// <para>
        /// Sets table name and schema, defines primary key, configures property constraints,
        /// establishes relationships, and creates indexes.
        /// </para>
        /// </summary>
        /// <param name="builder">
        /// The <see cref="EntityTypeBuilder{GenericAttribute}"/> instance used to configure the entity.
        /// Provides fluent API methods to define table mappings, property settings, relationships, and indexes.
        /// </param>
        public void Configure(EntityTypeBuilder<GenericAttribute> builder)
        {
            // -----------------------------
            // Table configuration
            // -----------------------------
            builder.ToTable("GenericAttributes", "Base"); // Table name: Base.GenericAttributes

            // -----------------------------
            // Primary key configuration
            // -----------------------------
            //builder.HasKey(a => a.Id)
            //       .HasName("PK_GenericAttributes"); // Explicit PK name for clarity

            // -----------------------------
            // Property configurations
            // -----------------------------
            builder.Property(a => a.Key)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasComment("Attribute key/name. Required. Maximum length of 100 characters.");

            builder.Property(a => a.Value)
                   .IsRequired()
                   .HasMaxLength(500)
                   .HasComment("Attribute value associated with the key. Required. Maximum length of 500 characters.");

            builder.Property(a => a.BaseEntityId)
                   .IsRequired()
                   .HasComment("Foreign key linking this attribute to its parent BaseEntity.");

            // -----------------------------
            // Relationship configuration
            // -----------------------------
            builder.HasOne(a => a.BaseEntity)           // Each GenericAttribute has one BaseEntity
                   .WithMany(b => b.GenericAttributes) // Each BaseEntity has many GenericAttributes
                   .HasForeignKey(a => a.BaseEntityId)
                   .OnDelete(DeleteBehavior.Cascade)  // Cascade delete to remove attributes when BaseEntity is deleted
                   .HasConstraintName("FK_GenericAttributes_BaseEntity");

            // -----------------------------
            // Index configuration
            // -----------------------------
            builder.HasIndex(a => new { a.BaseEntityId, a.Key })
                   .HasDatabaseName("IX_GenericAttributes_BaseEntityId_Key");
        }
    }
}
