using BaseProject.Application.Common.Interfaces;
using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Interfaces;
using System.Security.Claims;

namespace BaseProject.Application.Services
{
    public class CurrentUser(ITokenService tokenService, ICookieService cookieService , IUnitOfWork unitOfWork) : ICurrentUser
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly ICookieService _cookieService = cookieService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public string GetCurrentUserId()
        {
            var jwtCookie = _cookieService.Get();
            var token = _tokenService.ValidateToken(jwtCookie);
            var userId = token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return userId;
        }
        public async Task<ApplicationUser> GetCurrentUser()
        {
            var jwtCookie = _cookieService.Get();
            var token = _tokenService.ValidateToken(jwtCookie);
            var userId = token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return await _unitOfWork.Users.GetByIdAsync(userId);
        }
    }
}
