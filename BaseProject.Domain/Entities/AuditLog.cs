using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.Domain.Entities
{
    /// <summary>
    /// Logs audit information for entity changes.
    /// </summary>
    public class AuditLog : BaseEntity
    {
        [SwaggerSchema("Name of the entity being audited.")]
        public string EntityName { get; set; }

        [SwaggerSchema("Type of action performed, e.g., Insert, Update, Delete.")]
        public string ActionType { get; set; }

        [SwaggerSchema("Details of changes in JSON format.")]
        public string Changes { get; set; }

        [SwaggerSchema("User who performed the action.")]
        public string? UserId { get; set; }

        [SwaggerSchema("Timestamp of the action.")]
        public DateTime Timestamp { get; set; }
    }

}
