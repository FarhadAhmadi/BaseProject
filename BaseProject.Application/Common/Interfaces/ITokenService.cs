using BaseProject.Application.DTOs.AuthIdentity.UsersIdentity;
using BaseProject.Domain.Entities.Auth;
using System.Security.Claims;

namespace BaseProject.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
        ClaimsPrincipal ValidateToken(string token);
        Task<TokenResponseDto> GenerateToken(ApplicationUser user, string[] scopes, CancellationToken cancellationToken);
    }
}
