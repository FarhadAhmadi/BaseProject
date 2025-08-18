using MediatR;
using Serilog;

namespace BaseProject.Application.Features.Auth.Commands.SignUp;

public sealed class SignUpCommand : IRequest<SignUpResponse>
{
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
}