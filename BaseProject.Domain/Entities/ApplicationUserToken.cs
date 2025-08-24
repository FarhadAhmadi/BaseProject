using Microsoft.AspNetCore.Identity;

namespace BaseProject.Domain.Entities
{
    /// <summary>
    /// Represents a token for a user.
    /// </summary>
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public virtual ApplicationUser User { get; set; }

        #region Audit Properties
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public string? CreatorId { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string? UpdaterId { get; set; }
        #endregion
    }
}
