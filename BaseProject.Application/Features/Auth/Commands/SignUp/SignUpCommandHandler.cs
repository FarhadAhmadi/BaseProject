using AutoMapper;
using BaseProject.Application.Common.Exceptions;
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

    public SignUpCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Sign up started for {UserName} with email {Email}",
            request.UserName, request.Email);

        if (await _unitOfWork.Users.ExistsAsync(x => x.UserName == request.UserName
                                                  || x.Email == request.Email))
            throw UserException.UserAlreadyExistsException($"{request.UserName}/{request.Email}");

        var user = _mapper.Map<User>(request);
        user.Password = user.Password.HashPassword();

        await _unitOfWork.ExecuteInTransactionAsync(
            async () => await _unitOfWork.Users.AddAsync(user),
            cancellationToken
        );

        var response = _mapper.Map<SignUpResponse>(user);

        Log.Information("Sign up completed successfully | UserId: {UserId}", user.Id);

        return response;
    }
}
