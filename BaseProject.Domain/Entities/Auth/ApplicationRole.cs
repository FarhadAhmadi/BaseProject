using BaseProject.Domain.Entities.Authorization;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.Domain.Entities.Auth
{
    /// <summary>
    /// Represents an application role in the system.
    /// </summary>
    public class ApplicationRole : IdentityRole<string>
    {
        [SwaggerSchema("Active role.")]
        public bool Active { get; set; }

        [SwaggerSchema("Users assigned to this role.")]
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = [];

        [SwaggerSchema("Permission actions assigned to this role.")]
        public virtual ICollection<RolePermissionAction> RolePermissionActions { get; set; } = [];

        [SwaggerSchema("Claims assigned to this role.")]
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = [];

        #region Audit Properties
        public DateTimeOffset CreatedOn { get; set; } = DateTime.UtcNow;
        public string? CreatorId { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string? UpdaterId { get; set; }
        #endregion
    }
}