using BaseProject.Application.DTOs.Common.AuthIdentity.UsersIdentity;
using BaseProject.Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;

namespace BaseProject.Application.Common.Interfaces
{
    public interface IAuthIdentityService
    {
        Task LogOut();
        Task<TokenResultDto> RefreshTokenAsync(string token, CancellationToken cancellationToken);
        Task<TokenResultDto> Authenticate(DTOs.Common.AuthIdentity.UsersIdentity.LoginRequestDto request, CancellationToken cancellationToken);
        Task Register(DTOs.Common.AuthIdentity.UsersIdentity.RegisterRequestDto request, CancellationToken cancellationToken);
        Task<UserDto> Get(CancellationToken cancellationToken);
        Task<ForgotPassword> SendPasswordResetCode(SendPasswordResetCodeRequestDto request, CancellationToken cancellationToken);
        Task ResetPassword(DTOs.Common.AuthIdentity.UsersIdentity.ResetPasswordRequestDto request, CancellationToken cancellationToken);
        // Task<TokenResult> SignInFacebook(string accessToken, CancellationToken cancellationToken);
        // Task<TokenResult> SignInGoogle(string accessToken, CancellationToken cancellationToken);
        // Task<TokenResult> SignInApple(string fullName, string identityToken, CancellationToken cancellationToken);
    }
}
