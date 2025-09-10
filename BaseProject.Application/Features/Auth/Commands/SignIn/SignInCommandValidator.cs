using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Validation;
using BaseProject.Domain.Interfaces;

namespace BaseProject.Application.Features.Auth.Commands.SignIn
{
    public sealed class SignInCommandValidator : BusinessValidatorBase<SignInCommand>
    {
        public SignInCommandValidator(IUnitOfWork unitOfWork)
        {
            // Rule 1: User must exist
            RuleFor(
                async r => await unitOfWork.Users.ExistsAsync(u => u.UserName == r.UserName),
                () => AuthIdentityException.ThrowInvalidCredentials()
            );

            // Rule 2: Password cannot be empty (basic safeguard)
            RuleFor(
                r => Task.FromResult(!string.IsNullOrWhiteSpace(r.Password)),
                () => AuthIdentityException.ThrowInvalidCredentials()
            );
        }
    }
}