using BaseProject.Domain.Enums;

namespace BaseProject.Application.Features.Auth.Commands.SignUp;
public class SignUpResponse
{
    public string Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
}
