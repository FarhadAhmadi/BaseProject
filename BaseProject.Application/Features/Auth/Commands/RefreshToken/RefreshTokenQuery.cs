using AutoMapper;
using MediatR;
using Serilog;

namespace BaseProject.Application.Features.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommand : IRequest<RefreshTokenResponse>
{
    public string Token {  get; set; }
}