using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.Domain.Entities.Base
{
    /// <summary>
    /// Abstract parent entity that provides a unique GUID-based identifier.
    /// All domain entities inherit from this class to ensure consistent identity.
    /// </summary>
    public abstract class ParentEntity
    {
        /// <summary>
        /// Unique identifier of the entity.
        /// Automatically generated as a GUID string upon creation.
        /// Immutable from external code.
        /// </summary>
        [SwaggerSchema("Unique identifier of the entity, automatically generated as a GUID string.")]
        public string Id { get; private set; } = Guid.NewGuid().ToString();
    }
}
