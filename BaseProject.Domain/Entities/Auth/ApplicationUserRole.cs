using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.Domain.Entities.Auth
{
    /// <summary>
    /// Represents a mapping between user and role.
    /// </summary>
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        [SwaggerSchema("The user associated with this role mapping.")]
        public virtual ApplicationUser User { get; set; }

        [SwaggerSchema("The role associated with this user mapping.")]
        public virtual ApplicationRole Role { get; set; }

        #region Audit Properties
        public DateTimeOffset CreatedOn { get; set; } = DateTime.UtcNow;
        public string? CreatorId { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string? UpdaterId { get; set; }
        #endregion
    }
}
