using BaseProject.Application.DTOs.Common.AuthIdentity.UsersIdentity;
using BaseProject.Domain.Entities;
using System.Security.Claims;

namespace BaseProject.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        ClaimsPrincipal ValidateToken(string token);
        Task<TokenResultDto> GenerateToken(ApplicationUser user, string[] scopes, CancellationToken cancellationToken);
    }
}
