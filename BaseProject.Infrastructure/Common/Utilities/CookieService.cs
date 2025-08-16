using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace BaseProject.Infrastructure.Common.Utilities
{
    public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public void Set(string token)
            => _httpContextAccessor.HttpContext?.Response.Cookies.Append("token_key", token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                        Secure = true,
                        MaxAge = TimeSpan.FromMinutes(30)
                    });

        public void Delete() => _httpContextAccessor.HttpContext?.Response.Cookies.Delete("token_key");

        public string Get()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["token_key"];
            if (string.IsNullOrEmpty(token))
            {
                Log.Warning("No token found in cookies for current request.");
                throw UserException.UserUnauthorizedException();
            }

            return token;
        }
    }
}
