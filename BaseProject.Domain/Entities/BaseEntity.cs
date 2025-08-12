namespace BaseProject.Domain.Entities
{
    public abstract class BaseEntity
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public DateTimeOffset CreatedOn { get; set; }
        public string? CreatorId { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }
        public string? UpdaterId { get; set; }
    }
}
