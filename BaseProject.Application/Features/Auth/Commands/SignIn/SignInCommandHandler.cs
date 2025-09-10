using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Extensions.PredefinedLogs;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Features.Auth.Commands.SignIn;
using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public sealed class SignInCommandHandler : IRequestHandler<SignInCommand, SignInResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICookieService _cookieService;
    private readonly IAppLogger _appLogger;

    public SignInCommandHandler(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        ICookieService cookieService,
        IAppLogger appLogger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _cookieService = cookieService;
        _appLogger = appLogger;
    }

    public async Task<SignInResponseDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        // 1. Log: Start login attempt
        _appLogger.LogLoginAttempt(request.UserName);

        // 2. Try to find user (with roles and avatar for claims and profile completeness)
        var user = await _userManager.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.Avatar)
            .FirstOrDefaultAsync(u => u.UserName == request.UserName, cancellationToken);

        //if (user == null)
        //{
        //    // Log: User not found
        //    _appLogger.LogLoginInvalidUserName(request.UserName);
        //    throw AuthIdentityException.ThrowInvalidCredentials();
        //}

        // 3. Check password with lockout enabled
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            // Log: Wrong password attempt
            _appLogger.LogLoginInvalidPassword(user.Id);
            throw AuthIdentityException.ThrowLoginUnsuccessful();
        }

        // 4. Fetch roles and claims (scopes may be inside claims)
        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);
        var scopes = claims.FirstOrDefault(c => c.Type == "scope")?.Value.Split(' ') ?? Array.Empty<string>();

        // 5. Generate JWT & Refresh Token
        var tokenResponse = await _tokenService.GenerateToken(user, scopes, cancellationToken);

        // 6. Optionally store JWT inside HttpOnly cookie (for browser-based login)
        if (_cookieService != null)
        {
            _cookieService.Delete();
            _cookieService.Set(tokenResponse.Token);
        }

        // 7. Log: Successful login
        _appLogger.LogLoginResult(user.Id, success: true);

        // 8. Return response DTO
        return new SignInResponseDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Token = tokenResponse.Token,
        };
    }
}
