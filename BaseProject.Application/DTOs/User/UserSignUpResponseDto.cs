using BaseProject.Domain.Enums;

namespace BaseProject.Application.DTOs.User;
public class UserSignUpResponseDto
{
    public string Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
}
