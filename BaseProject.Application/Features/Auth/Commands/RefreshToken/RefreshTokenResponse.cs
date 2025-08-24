namespace BaseProject.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}
