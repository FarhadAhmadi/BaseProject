using MediatR;

namespace BaseProject.Application.Features.Auth.Commands.SignIn;

public sealed class SignInCommand : IRequest<SignInResponse>
{
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
