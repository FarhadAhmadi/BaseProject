using AutoMapper;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Common.Utilities;
using BaseProject.Domain.Constants;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using MediatR;
using Serilog;

namespace BaseProject.Application.Features.Auth.Commands.SignIn;

public sealed partial class SignInCommandHandler
    : IRequestHandler<SignInCommand, SignInResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ICookieService _cookieService;
    private readonly IMapper _mapper;

    public SignInCommandHandler(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        ICookieService cookieService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _cookieService = cookieService;
        _mapper = mapper;
    }

    public async Task<SignInResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Sign in started for {UserName}", request.UserName);

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

        if (user == null)
            throw UserException.BadRequestException(UserErrorMessage.UserNotExist);

        Log.Debug("User found | UserId: {UserId}", user.Id);

        if (!StringHelper.VerifyPassword(request.Password, user.Password))
            throw UserException.BadRequestException(UserErrorMessage.PasswordIncorrect);

        Log.Debug("Password verified successfully | UserId: {UserId}", user.Id);

        var token = _tokenService.GenerateToken(user);
        _cookieService.Set(token);

        Log.Debug("Token generated and set in cookie | UserId: {UserId} | TokenLength: {TokenLength}",
            user.Id, token.Length);

        var response = _mapper.Map<SignInResponse>(user);
        response.Token = token;

        Log.Information("Sign in completed successfully | UserId: {UserId}", user.Id);

        return response;
    }
}
