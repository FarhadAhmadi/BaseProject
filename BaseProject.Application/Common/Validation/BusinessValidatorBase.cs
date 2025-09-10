using BaseProject.Application.Common.Exceptions;
using BaseProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Application.Common.Validation
{
    public abstract class BusinessValidatorBase<T> : IBusinessValidator<T>
    {
        private readonly List<Func<T, Task<Exception?>>> _rules = new();

        public async Task<ValidationResult> ValidateAsync(T request, bool throwOnFailure = false)
        {
            var errors = new List<Exception>();
            foreach (var rule in _rules)
            {
                var result = await rule(request);
                if (result != null)
                {
                    if (throwOnFailure)
                        throw result; // stop at first failure
                    errors.Add(result);
                }
            }

            if (errors.Any())
                return ValidationResult.Failure(errors.Select(e => e.Message).ToList());

            return ValidationResult.Success();
        }

        // Rule that uses an error message (old style)
        public void RuleFor(Func<T, Task<bool>> predicate, string errorMessage)
        {
            _rules.Add(async x => await predicate(x)
                ? null
                : new FriendlyException(ApiErrorCode.BadRequest, errorMessage, errorMessage));
        }
        public void RuleFor(Func<T, Task<bool>> predicate, Func<Exception> exceptionFactory)
        {
            _rules.Add(async x => await predicate(x)
                ? null
                : exceptionFactory());
        }
    }
}
