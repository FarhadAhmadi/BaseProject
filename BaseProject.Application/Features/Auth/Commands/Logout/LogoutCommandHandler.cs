using BaseProject.Application.Common.Interfaces;
using MediatR;
using Serilog;

namespace BaseProject.Application.Features.Auth.Commands.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly ICookieService _cookieService;

    public LogoutCommandHandler(ICurrentUser currentUser, ICookieService cookieService)
    {
        _currentUser = currentUser;
        _cookieService = cookieService;
    }

    public Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetCurrentUserId();
        _cookieService.Delete();
        Log.Information("User {UserId} logged out successfully", userId);

        return Task.CompletedTask;
    }
}
