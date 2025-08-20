using AutoMapper;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Extensions.PredefinedLogs;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Common.Utilities;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using MediatR;

namespace BaseProject.Application.Features.Auth.Commands.SignUp;

public sealed class SignUpCommandHandler
    : IRequestHandler<SignUpCommand, SignUpResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAppLogger _appLogger;

    public SignUpCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAppLogger appLogger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _appLogger = appLogger;
    }

    public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        _appLogger.LogSignUpAttempt(request.UserName, request.Email);

        if (await _unitOfWork.Users.ExistsAsync(x => x.UserName == request.UserName || x.Email == request.Email))
        {
            // Use predefined warning log pattern
            _appLogger.LogSignUpResult(request.UserName,false);
            throw UserException.UserAlreadyExistsException($"{request.UserName}/{request.Email}");
        }

        var user = _mapper.Map<User>(request);
        user.Password = user.Password.HashPassword();

        await _unitOfWork.ExecuteInTransactionAsync(
            async () => await _unitOfWork.Users.AddAsync(user),
            cancellationToken
        );

        var response = _mapper.Map<SignUpResponse>(user);
        // Use predefined warning log pattern
        _appLogger.LogSignUpResult(request.UserName, true);

        return response;
    }
}
