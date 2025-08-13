namespace BaseProject.Shared.DTOs.Common
{
    public abstract class BaseDTO
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public DateTimeOffset? CreatedOn { get; set; }
        public string? CreatorId { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }
        public string? UpdaterId { get; set; }
    }
}

