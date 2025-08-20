using BaseProject.Application.Common.Extensions.PredefinedLogs;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Domain.Interfaces;
using MediatR;

namespace BaseProject.Application.Features.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ICookieService _cookieService;
    private readonly IAppLogger _appLogger;

    public RefreshTokenCommandHandler(
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        ICookieService cookieService,
        IAppLogger appLogger)
    {
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _cookieService = cookieService;
        _appLogger = appLogger;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetCurrentUserId();
        var user = await _unitOfWork.Users.GetByIdAsync(userId);

        var accessToken = _tokenService.GenerateToken(user);
        _cookieService.Set(accessToken);

        _appLogger.LogTokenRefresh(userId);

        return new RefreshTokenResponse
        {
            AccessToken = accessToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1) // example
        };
    }
}
