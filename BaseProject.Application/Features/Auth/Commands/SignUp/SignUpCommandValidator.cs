using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Validation;
using BaseProject.Application.Features.Auth.Commands.SignUp;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

public sealed class SignUpCommandValidator : BusinessValidatorBase<SignUpCommand>
{
    public SignUpCommandValidator(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        // 1. Password match
        RuleFor(r => Task.FromResult(r.Password == r.RePassword),
            () => AuthIdentityException.ThrowPasswordMismatch()
        );

        // 2. Username availability
        RuleFor(async r => (await userManager.FindByNameAsync(r.UserName)) == null,
                        () => AuthIdentityException.ThrowUsernameAvailable()
        );

        // 3. Email availability
        RuleFor(async r => (await userManager.FindByEmailAsync(r.Email)) == null,
                        () => AuthIdentityException.ThrowEmailAvailable()
        );

        // 4. Phone number unique (optional example if required)
        RuleFor(async r => string.IsNullOrEmpty(r.PhoneNumber) ||
                           !(await unitOfWork.Users.ExistsAsync(u => u.PhoneNumber == r.PhoneNumber)),
                        () => AuthIdentityException.ThrowPhoneNumberAvailable()
        );
    }
}
