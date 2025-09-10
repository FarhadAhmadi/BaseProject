using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.Domain.Entities.Base
{
    /// <summary>
    /// Represents the base class for all domain entities.
    /// Provides auditing fields, metadata, and a collection of dynamic attributes.
    /// Serves as the foundation for business entities in the system.
    /// </summary>
    public abstract class BaseEntity : ParentEntity
    {
        /// <summary>
        /// Gets or sets the UTC timestamp when the entity was created.
        /// Defaults to the current UTC time when the entity is instantiated.
        /// </summary>
        [SwaggerSchema("Entity creation timestamp in UTC.")]
        public DateTimeOffset CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the identifier of the user who created this entity.
        /// Useful for auditing purposes.
        /// </summary>
        [SwaggerSchema("User ID of the entity creator.")]
        public string? CreatorId { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp of the last update.
        /// Null if the entity has never been updated.
        /// </summary>
        [SwaggerSchema("Entity last update timestamp in UTC (null if never updated).")]
        public DateTimeOffset? UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who last updated the entity.
        /// Null indicates the entity has never been updated.
        /// </summary>
        [SwaggerSchema("User ID of the entity updater (null if never updated).")]
        public string? UpdaterId { get; set; }

        /// <summary>
        /// A collection of dynamic key-value attributes associated with the entity.
        /// Allows extending the entity with additional metadata without altering its schema.
        /// </summary>
        [SwaggerSchema("Custom attributes attached to the entity as key-value pairs.")]
        public ICollection<GenericAttribute> GenericAttributes { get; set; } = new List<GenericAttribute>();
    }
}
