using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.Application.DTOs.AuthIdentity.UsersIdentity;

[SwaggerSchema("Represents the login request model.")]
public class LoginRequestDto
{
    [SwaggerSchema("The username of the user.")]
    public string UserName { get; set; }

    [SwaggerSchema("The password of the user.")]
    public string Password { get; set; }

    [SwaggerSchema("Indicates whether to remember the user for future logins.")]
    public bool RememberMe { get; set; }
}
