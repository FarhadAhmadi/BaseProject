using BaseProject.Application.Common.Extensions.PredefinedLogs;
using BaseProject.Application.Common.Interfaces;
using MediatR;
using Serilog;

namespace BaseProject.Application.Features.Auth.Commands.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly ICookieService _cookieService;
    private readonly IAppLogger _appLogger;

    public LogoutCommandHandler(ICurrentUser currentUser, ICookieService cookieService, IAppLogger appLogger)
    {
        _currentUser = currentUser;
        _cookieService = cookieService;
        _appLogger = appLogger;
    }

    public Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetCurrentUserId();
        _cookieService.Delete();

        _appLogger.LogLogout(userId);

        return Task.CompletedTask;
    }
}
