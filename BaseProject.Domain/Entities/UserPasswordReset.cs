using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Entities.Base;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace BaseProject.Domain.Entities
{
    /// <summary>
    /// Represents a password reset request for a user.
    /// </summary>
    public class UserPasswordReset : BaseEntity
    {
        [SwaggerSchema("The ID of the user requesting the password reset.")]
        public string UserId { get; set; }

        [SwaggerSchema("Email of the user.")]
        public string Email { get; set; }

        [SwaggerSchema("Token generated for password reset.")]
        public string Token { get; set; }

        [SwaggerSchema("One-time password for verification.")]
        public string OTP { get; set; }

        [SwaggerSchema("Date and time of the reset request.")]
        public DateTime DateTime { get; set; }

        [SwaggerSchema("The user associated with this password reset.")]
        public ApplicationUser User { get; set; }
    }
}
