using AutoMapper;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Extensions.PredefinedLogs;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Common.Utilities;
using BaseProject.Domain.Constants;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BaseProject.Application.Features.Auth.Commands.SignIn;

public sealed class SignInCommandHandler
    : IRequestHandler<SignInCommand, SignInResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ICookieService _cookieService;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAppLogger _appLogger;


    public SignInCommandHandler(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        ICookieService cookieService,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IAppLogger appLogger)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _cookieService = cookieService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _appLogger = appLogger;
    }

    public async Task<SignInResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetFirstOrDefaultAsync<User>(
            filter: x => x.UserName == request.UserName,
            selector: x => new User
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                Password = x.Password
            }
        );

        var invalidMessage = "Invalid username or password.";

        _appLogger.LogLoginAttempt(request.UserName);

        if (user == null || !StringHelper.VerifyPassword(request.Password, user.Password))
        {
            _appLogger.LogLoginResult(user?.Id,false);
            throw UserException.BadRequestException(invalidMessage);
        }

        _appLogger.LogLoginResult(user?.Id, true);

        var token = _tokenService.GenerateToken(user);
        _cookieService.Set(token);

        var response = _mapper.Map<SignInResponse>(user);
        response.Token = token;

        return response;

    }
}
