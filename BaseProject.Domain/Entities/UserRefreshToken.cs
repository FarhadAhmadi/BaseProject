using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Entities.Base;
using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.Domain.Entities
{
    /// <summary>
    /// Represents a refresh token issued to a user for authentication.
    /// </summary>
    public class UserRefreshToken : BaseEntity
    {
        [SwaggerSchema("The token string.")]
        public string Token { get; set; }

        [SwaggerSchema("Expiration date of the token.")]
        public DateTime Expires { get; set; }

        [SwaggerSchema("Indicates whether the token is expired.")]
        public bool IsExpired => DateTime.UtcNow >= Expires;

        [SwaggerSchema("Date the token was created.")]
        public DateTime Created { get; set; }

        [SwaggerSchema("Date the token was revoked, if any.")]
        public DateTime? Revoked { get; set; }

        [SwaggerSchema("Indicates whether the token is currently active.")]
        public bool IsActive => Revoked == null && !IsExpired;

        [SwaggerSchema("The ID of the user associated with this token.")]
        public string UserId { get; set; }

        [SwaggerSchema("The user associated with this token.")]
        public ApplicationUser User { get; set; }
    }
}
