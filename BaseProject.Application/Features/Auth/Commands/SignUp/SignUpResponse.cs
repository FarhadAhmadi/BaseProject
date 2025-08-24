using BaseProject.Domain.Enums;

namespace BaseProject.Application.Features.Auth.Commands.SignUp;
public class SignUpResponse
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
}
