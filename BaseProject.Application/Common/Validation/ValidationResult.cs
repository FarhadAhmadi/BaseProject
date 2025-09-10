using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Application.Common.Validation
{
    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public List<string> Errors { get; }

        private ValidationResult(List<string> errors) => Errors = errors;

        public static ValidationResult Success() => new(new());
        public static ValidationResult Failure(IEnumerable<string> errors)
            => new(errors.ToList());
    }
}
