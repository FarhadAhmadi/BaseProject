using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.Application.DTOs.Common.AuthIdentity.UsersIdentity;

[SwaggerSchema("Represents the reset password request model.")]
public class ResetPasswordRequestDto
{
    [SwaggerSchema("The email address of the user.")]
    public string Email { get; set; }

    [SwaggerSchema("The one-time password (OTP) sent to the user.")]
    public string OTP { get; set; }

    [SwaggerSchema("The new password for the account.")]
    public string NewPassword { get; set; }
}
