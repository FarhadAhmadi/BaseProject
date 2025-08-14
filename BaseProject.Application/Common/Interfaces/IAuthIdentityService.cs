using BaseProject.Application.DTOs.AuthIdentity.UsersIdentity;
using BaseProject.Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;

namespace BaseProject.Application.Common.Interfaces
{
    public interface IAuthIdentityService
    {
        Task LogOut();
        Task<TokenResponseDto> RefreshTokenAsync(string token, CancellationToken cancellationToken);
        Task<TokenResponseDto> Authenticate(DTOs.AuthIdentity.UsersIdentity.LoginRequestDto request, CancellationToken cancellationToken);
        Task Register(DTOs.AuthIdentity.UsersIdentity.RegisterRequestDto request, CancellationToken cancellationToken);
        Task<UserResponseDto> Get(CancellationToken cancellationToken);
        Task<ForgotPassword> SendPasswordResetCode(SendPasswordResetCodeRequestDto request, CancellationToken cancellationToken);
        Task ResetPassword(DTOs.AuthIdentity.UsersIdentity.ResetPasswordRequestDto request, CancellationToken cancellationToken);
        // Task<TokenResult> SignInFacebook(string accessToken, CancellationToken cancellationToken);
        // Task<TokenResult> SignInGoogle(string accessToken, CancellationToken cancellationToken);
        // Task<TokenResult> SignInApple(string fullName, string identityToken, CancellationToken cancellationToken);
    }
}
