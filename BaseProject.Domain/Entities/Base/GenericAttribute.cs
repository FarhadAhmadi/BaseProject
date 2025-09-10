using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.Domain.Entities.Base
{
    /// <summary>
    /// Represents a dynamic key-value attribute that can be attached to a <see cref="BaseEntity"/>.
    /// Useful for extending entities without changing the database schema.
    /// </summary>
    public class GenericAttribute : ParentEntity
    {
        /// <summary>
        /// The key or name of the attribute (e.g., "Color", "Theme", "Tag").
        /// </summary>
        [SwaggerSchema("Key (name) of the attribute.")]
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// The value associated with the attribute key.
        /// Stores the actual metadata or configuration value.
        /// </summary>
        [SwaggerSchema("Value of the attribute associated with the key.")]
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key referencing the parent <see cref="BaseEntity"/> this attribute belongs to.
        /// </summary>
        [SwaggerSchema("Foreign key linking this attribute to its parent BaseEntity.")]
        public string BaseEntityId { get; set; }

        /// <summary>
        /// Navigation property to the parent <see cref="BaseEntity"/>.
        /// Enables EF Core relationship mapping and lazy/eager loading.
        /// </summary>
        [SwaggerSchema("Reference to the parent BaseEntity.")]
        public BaseEntity BaseEntity { get; set; } = null!;
    }
}