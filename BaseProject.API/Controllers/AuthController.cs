using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.DTOs.User;
using BaseProject.Shared.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.API.Controllers
{
    [Route("api/[controller]/")]
    public class AuthController(IAuthService authService) : BaseController
    {
        private readonly IAuthService _userService = authService;

        /// <summary>
        /// Sign in
        /// </summary>
        [HttpPost("sign-in")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully signed in.", typeof(ResponseDto<UserSignInResponseDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request.", typeof(ResponseDto<object>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication failed.", typeof(ResponseDto<object>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ResponseDto<object>))]
        public async Task<IActionResult> SignIn(UserSignInRequestDto request)
        {
            var result = await _userService.SignIn(request);
            return result == null
                ? Fail<UserSignInResponseDto>("Authentication failed", StatusCodes.Status401Unauthorized)
                : Success(result, "Successfully signed in");
        }

        /// <summary>
        /// Sign up
        /// </summary>
        [HttpPost("sign-up")]
        [SwaggerResponse(StatusCodes.Status201Created, "User registered successfully.", typeof(ResponseDto<UserSignUpResponseDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request.")]
        public async Task<IActionResult> SignUp(UserSignUpRequestDto request, CancellationToken token)
        {
            var result = await _userService.SignUp(request, token);
            return result == null
                ? Fail<UserSignUpResponseDto>("Registration failed", StatusCodes.Status400BadRequest)
                : StatusCode(StatusCodes.Status201Created, ResponseDto<UserSignUpResponseDto>.SuccessResponse(result, "User registered successfully"));
        }

        /// <summary>
        /// Logout
        /// </summary>
        [HttpDelete("logout")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully logged out.", typeof(ResponseDto<string>))]
        public IActionResult Logout()
        {
            _userService.Logout();
            return Success("Successfully logged out", "Logout successful");
        }

        /// <summary>
        /// Refresh a token
        /// </summary>
        [HttpGet("refresh")]
        [SwaggerResponse(StatusCodes.Status200OK, "Token refreshed successfully.", typeof(ResponseDto<string>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Refresh token is invalid or expired.")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = await _userService.RefreshToken();
            return string.IsNullOrEmpty(token)
                ? Fail<string>("Refresh token is invalid or expired", StatusCodes.Status401Unauthorized)
                : Success(token, "Token refreshed successfully");
        }

        /// <summary>
        /// Get profile information
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved user profile.", typeof(ResponseDto<UserProfileResponseDto>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authorized.")]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _userService.GetProfile();
            return profile == null
                ? Fail<UserProfileResponseDto>("Profile not found", StatusCodes.Status404NotFound)
                : Success(profile, "Profile retrieved successfully");
        }
    }
}
