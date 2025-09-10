using BaseProject.Domain.Enums;

namespace BaseProject.Application.Features.Auth.Commands.SignIn;

public sealed record SignInResponseDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public string Token { get; set; }
}
