using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseProject.Domain.Entities.Auth
{
    /// <summary>
    /// Represents an application role in the system.
    /// </summary>
    public class ApplicationRole : IdentityRole<string>
    {
        [SwaggerSchema("Users assigned to this role.")]
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }

        #region Audit Properties
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public string? CreatorId { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string? UpdaterId { get; set; }
        #endregion
    }
}