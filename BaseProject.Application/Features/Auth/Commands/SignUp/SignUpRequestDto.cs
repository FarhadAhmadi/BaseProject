using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BaseProject.Application.Features.Auth.Commands.SignUp;

public sealed class SignUpRequestDto
{
    [Required(ErrorMessage = "FullName is required.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string RePassword { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email is not valid.")]
    public string Email { get; set; }

    [RegularExpression(@"^(?:\+98|0)?9\d{9}$", ErrorMessage = "PhoneNumber must be a valid Iranian mobile number.")]
    public string PhoneNumber { get; set; }
}
