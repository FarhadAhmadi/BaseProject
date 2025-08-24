using MediatR;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace BaseProject.Application.Features.Auth.Commands.SignUp;

public sealed class SignUpCommand : IRequest<SignUpResponse>
{
    public string UserName { get; init; }
    public string Password { get; init; }
    public string RePassword { get; init; }
    public string Email { get; init; }
    public string? PhoneNumber { get; init; }
}