using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.Domain.Entities
{
    /// <summary>
    /// Base entity with common properties for all entities.
    /// </summary>
    public abstract class BaseEntity
    {
        [SwaggerSchema("Unique identifier of the entity.")]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [SwaggerSchema("Entity creation timestamp.")]
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;

        [SwaggerSchema("User who created this entity.")]
        public string? CreatorId { get; set; }

        [SwaggerSchema("Last update timestamp of the entity.")]
        public DateTimeOffset? UpdatedOn { get; set; }

        [SwaggerSchema("User who last updated this entity.")]
        public string? UpdaterId { get; set; }
    }
}
