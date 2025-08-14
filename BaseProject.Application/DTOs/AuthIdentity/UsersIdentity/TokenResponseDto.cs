namespace BaseProject.Application.DTOs.AuthIdentity.UsersIdentity;

public class TokenResponseDto
{
    public string UserId { get; set; }

    public string Token { get; set; }

    public DateTime Expires { get; set; }
}
