using Microsoft.AspNetCore.Identity;

namespace BaseProject.Domain.Entities.Auth
{
    /// <summary>
    /// Represents a claim associated with a role.
    /// </summary>
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public virtual ApplicationRole Role { get; set; }

        #region Audit Properties
        public DateTimeOffset CreatedOn { get; set; } = DateTime.UtcNow;
        public string? CreatorId { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string? UpdaterId { get; set; }
        #endregion
    }
}
