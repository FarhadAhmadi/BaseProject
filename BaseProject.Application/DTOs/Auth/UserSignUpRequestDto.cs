using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BaseProject.Application.DTOs.User;

public class UserSignUpRequestDto
{
    public string UserName { get; set; }

    [DataType(DataType.Password)]
    [SwaggerSchema(Format = "password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [SwaggerSchema(Format = "password")]
    public string RePassword { get; init; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
}
