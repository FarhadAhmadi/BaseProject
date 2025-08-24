using BaseProject.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace BaseProject.Domain.Entities
{
    /// <summary>
    /// Represents an application user in the system.
    /// </summary>
    public class ApplicationUser : IdentityUser<string>
    {
        [PersonalData]
        [SwaggerSchema("The full name of the user.")]
        public string FullName { get; set; }


        [PersonalData]
        [SwaggerSchema("The status of the user account.")]
        public Status? Status { get; set; }


        [PersonalData]
        [SwaggerSchema("The ID of the user's avatar media, if available.")]
        public string? AvatarId { get; set; }


        [PersonalData]
        [SwaggerSchema("The avatar media associated with the user.")]
        public UserMedia? Avatar { get; set; }


        [SwaggerSchema("Roles assigned to the user.")]
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();


        [SwaggerSchema("Claims associated with the user.")]
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; } = new List<ApplicationUserClaim>();


        [SwaggerSchema("External login providers linked to the user.")]
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; } = new List<ApplicationUserLogin>();


        [SwaggerSchema("Authentication tokens for the user.")]
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; } = new List<ApplicationUserToken>();


        [SwaggerSchema("Refresh tokens issued to the user.")]
        public virtual ICollection<UserRefreshToken> RefreshTokens { get; set; } = new List<UserRefreshToken>();


        [SwaggerSchema("Password reset requests associated with the user.")]
        public virtual ICollection<UserPasswordReset> PasswordResetRequests { get; set; } = new List<UserPasswordReset>();

        #region Audit Properties
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public string? CreatorId { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string? UpdaterId { get; set; }
        #endregion
    }
}
