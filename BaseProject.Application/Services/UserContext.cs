using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Helpers;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Security.Claims;

namespace BaseProject.Application.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public string GetCurrentUserId()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"];
            Console.WriteLine("Authorization Header: " + authorizationHeader);

            var userClaims = _httpContextAccessor.HttpContext?.User;

            if (userClaims == null)
            {
                return null;
            }

            // Log or debug to check all available claims
            var claims = userClaims.Claims.Select(c => new { c.Type, c.Value });
            foreach (var claim in claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            var userIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value; // Return the UserId or null if not found
        }
    }
}
