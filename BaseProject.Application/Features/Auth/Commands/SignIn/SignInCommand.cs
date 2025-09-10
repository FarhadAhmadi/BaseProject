using MediatR;

namespace BaseProject.Application.Features.Auth.Commands.SignIn;

public sealed class SignInCommand : IRequest<SignInResponseDto>
{
    public string UserName { get; init; }
    public string Password { get; init; }
}
