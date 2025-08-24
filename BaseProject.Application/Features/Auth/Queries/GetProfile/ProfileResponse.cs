using BaseProject.Domain.Enums;

namespace BaseProject.Application.Features.Auth.Queries.GetProfile;

public class ProfileResponse
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
}