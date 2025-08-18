using BaseProject.Domain.Enums;

namespace BaseProject.Application.Features.Auth.Commands.SignIn;

public class SignInResponse
{
    public string Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
    public string Token { get; set; } = string.Empty;
}
