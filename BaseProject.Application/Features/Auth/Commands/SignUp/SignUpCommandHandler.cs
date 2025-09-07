using AutoMapper;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Extensions.PredefinedLogs;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Common.Utilities;
using BaseProject.Domain.Configurations;
using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Enums;
using BaseProject.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BaseProject.Application.Features.Auth.Commands.SignUp;

public sealed class SignUpCommandHandler
    : IRequestHandler<SignUpCommand, SignUpResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppSettings _appSettings;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAppLogger _appLogger;

    public SignUpCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAppLogger appLogger,
        UserManager<ApplicationUser> userManager,
        AppSettings appSettings)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _appLogger = appLogger;
        _userManager = userManager;
        _appSettings = appSettings;
    }

    public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        // 1. Log: Start signup attempt
        _appLogger.LogSignUpAttempt(request.UserName, request.Email);

        // 2. Validate password match
        if (request.Password != request.RePassword)
        {
            _appLogger.Warning("Sign-up failed | Passwords do not match | UserName: {UserName}", request.UserName);
            throw AuthIdentityException.ThrowPasswordMismatch();
        }

        // 3. Check if username or email already exists
        if (await _userManager.FindByNameAsync(request.UserName) != null)
        {
            _appLogger.Warning("Sign-up failed | Username already exists | UserName: {UserName}", request.UserName);
            throw AuthIdentityException.ThrowUsernameAvailable();
        }

        if (await _userManager.FindByEmailAsync(request.Email) != null)
        {
            _appLogger.Warning("Sign-up failed | Email already exists | Email: {Email}", request.Email);
            throw AuthIdentityException.ThrowEmailAvailable();
        }

        // 4. Create new user
        var user = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            FullName = request.FullName,
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = !string.IsNullOrEmpty(request.PhoneNumber) ? request.PhoneNumber : null
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _appLogger.Warning("Sign-up failed | User creation error | UserName: {UserName} | Errors: {Errors}",
                request.UserName, errors);
            throw AuthIdentityException.ThrowRegisterUnsuccessful(errors);
        }

        // 5. Assign default role
        await _userManager.AddToRoleAsync(user, Role.User.ToString());

        // 6. Assign default scopes
        string readScope = _appSettings.Identity.ScopeBaseDomain + "/read";
        string writeScope = _appSettings.Identity.ScopeBaseDomain + "/write";
        var scopes = new[] { readScope, writeScope };
        var scopeClaim = new Claim("scope", string.Join(" ", scopes));
        await _userManager.AddClaimAsync(user, scopeClaim);

        // 7. Map response DTO
        var response = _mapper.Map<SignUpResponse>(user);

        // 8. Log: Successful signup
        _appLogger.LogSignUpResult(request.UserName, true);

        return response;
    }
}
