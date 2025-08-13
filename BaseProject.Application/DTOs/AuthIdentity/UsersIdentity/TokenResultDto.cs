namespace BaseProject.Application.DTOs.Common.AuthIdentity.UsersIdentity;

public class TokenResultDto
{
    public string UserId { get; set; }

    public string Token { get; set; }

    public DateTime Expires { get; set; }
}
