namespace BaseProject.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
