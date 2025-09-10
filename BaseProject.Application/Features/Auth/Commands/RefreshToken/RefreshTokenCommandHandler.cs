using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Features.Auth.Commands.RefreshToken;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ICookieService _cookieService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAppLogger _appLogger;

    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        ICookieService cookieService,
        UserManager<ApplicationUser> userManager,
        IAppLogger appLogger)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _cookieService = cookieService;
        _userManager = userManager;
        _appLogger = appLogger;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 1️⃣ پیدا کردن refresh token از دیتابیس
        var refreshToken = await _unitOfWork.RefreshTokens
            .GetFirstOrDefaultAsync<UserRefreshToken>(
                r => r.Token == request.Token);

        if (refreshToken == null || !refreshToken.IsActive)
            throw AuthIdentityException.ThrowTokenNotActive();

        // 2️⃣ پیدا کردن کاربر مرتبط
        var user = await _userManager.Users
            .SingleOrDefaultAsync(u => u.Id == refreshToken.UserId, cancellationToken);

        if (user == null)
            throw AuthIdentityException.ThrowInvalidCredentials();

        // 3️⃣ revoke توکن فعلی
        refreshToken.Revoked = DateTime.UtcNow;

        // 4️⃣ ساخت access token جدید و در صورت لزوم rotate refresh token
        var roles = await _userManager.GetRolesAsync(user);
        var scopes = new string[] { }; // یا از claim قبلی استخراج کن
        var result = await _tokenService.GenerateToken(user, scopes, cancellationToken);

        // 5️⃣ تنظیم cookie
        _cookieService.Delete();
        _cookieService.Set(result.Token); // اینجا می‌تونی Access یا Refresh Token قرار بدی

        return new RefreshTokenResponse
        {
            AccessToken = result.Token,
            ExpiresAt = result.Expires
        };
    }
}
