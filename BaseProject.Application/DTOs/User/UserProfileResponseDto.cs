using BaseProject.Domain.Enums;

namespace BaseProject.Application.DTOs.User;

public class UserProfileResponseDto
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Role Role { get; set; }
}
