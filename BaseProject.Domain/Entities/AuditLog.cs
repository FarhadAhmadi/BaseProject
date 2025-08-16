namespace BaseProject.Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        public string EntityName { get; set; }
        public string ActionType { get; set; } // Insert, Update, Delete
        public string Changes { get; set; }    // Store changes in JSON format
        public string? UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
