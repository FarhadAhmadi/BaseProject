using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Application.Common.Validation
{
    public interface IBusinessValidator<T>
    {
        Task<ValidationResult> ValidateAsync(T request, bool throwOnFailure = false);

        void RuleFor(Func<T, Task<bool>> predicate, string errorMessage);

        // Rule that uses a specific exception factory (new style)
        void RuleFor(Func<T, Task<bool>> predicate, Func<Exception> exceptionFactory);
    }
}
