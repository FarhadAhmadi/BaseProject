using BaseProject.Domain.Enums;

namespace BaseProject.Application.Features.Auth.Queries.GetProfile;

public class ProfileResponse
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
}