using BaseProject.Application.Features.Auth.Commands.RefreshToken;
using BaseProject.Application.Features.Auth.Commands.SignIn;
using BaseProject.Application.Features.Auth.Commands.SignUp;
using BaseProject.Application.Features.Auth.Queries.GetProfile;
using BaseProject.Shared.DTOs.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BaseProject.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Sign in
        /// </summary>
        [HttpPost("sign-in")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully signed in.", typeof(ResponseDto<SignInResponseDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request.", typeof(ResponseDto<object>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication failed.", typeof(ResponseDto<object>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ResponseDto<object>))]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDto request, CancellationToken token)
        {
            var result = await _mediator.Send(new SignInCommand
            {
                UserName = request.UserName,
                Password = request.Password
            }, token);

            return result == null
                ? Fail<SignInResponseDto>("Authentication failed", StatusCodes.Status401Unauthorized)
                : Success(result, "Successfully signed in");
        }

        /// <summary>
        /// Sign up
        /// </summary>
        [HttpPost("sign-up")]
        [SwaggerResponse(StatusCodes.Status201Created, "User registered successfully.", typeof(ResponseDto<SignUpResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request.")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto request, CancellationToken token)
        {
            var result = await _mediator.Send(new SignUpCommand
            {
                FullName = request.FullName,
                UserName = request.UserName,
                Password = request.Password,
                RePassword = request.RePassword,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            }, token);

            return result == null
                ? Fail<SignUpResponse>("Registration failed", StatusCodes.Status400BadRequest)
                : StatusCode(StatusCodes.Status201Created,
                    ResponseDto<SignUpResponse>.SuccessResponse(result, "User registered successfully"));
        }

        /// <summary>
        /// Logout
        /// </summary>
        [HttpDelete("logout")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully logged out.", typeof(ResponseDto<string>))]
        public IActionResult Logout()
        {
            // If you want logout to also be CQRS, you’d need a LogoutCommand & handler
            // For now just return success
            return Success("Successfully logged out", "Logout successful");
        }

        /// <summary>
        /// Refresh a token
        /// </summary>
        [HttpPost("refresh")]
        [SwaggerResponse(StatusCodes.Status200OK, "Token refreshed successfully.", typeof(ResponseDto<string>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Refresh token is invalid or expired.")]
        public async Task<IActionResult> RefreshToken([FromBody]string token ,CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new RefreshTokenCommand() { Token = token}, cancellationToken);

            return response == null || string.IsNullOrEmpty(response.AccessToken)
                ? Fail<string>("Refresh token is invalid or expired", StatusCodes.Status401Unauthorized)
                : Success(response.AccessToken, "Token refreshed successfully");
        }

        /// <summary>
        /// Get profile information
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved user profile.", typeof(ResponseDto<ProfileResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User is not authorized.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Profile not found.")]
        public async Task<IActionResult> GetProfile(CancellationToken token)
        {
            var profile = await _mediator.Send(new GetProfileQuery(), token);

            return profile == null
                ? Fail<ProfileResponse>("Profile not found", StatusCodes.Status404NotFound)
                : Success(profile, "Profile retrieved successfully");
        }
    }
}
