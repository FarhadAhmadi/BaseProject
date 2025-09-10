using System.ComponentModel.DataAnnotations;

namespace BaseProject.Application.Features.Auth.Commands.SignIn;

public sealed class SignInRequestDto
{
    [Required(ErrorMessage = "UserName is required.")]
    [MinLength(3, ErrorMessage = "UserName must be at least 3 characters long.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }
}
