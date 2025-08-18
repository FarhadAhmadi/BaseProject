using BaseProject.Domain.Enums;
using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace BaseProject.Application.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public static class ValidationException
    {
        /// <summary>
        /// Converts FluentValidation failures into a FriendlyException
        /// </summary>
        /// <param name="failures">Validation failures</param>
        /// <returns>FriendlyException with combined messages</returns>
        public static FriendlyException FromFailures(IEnumerable<ValidationFailure> failures)
        {
            var messages = failures
                .Select(f => $"{f.ErrorMessage}")
                .ToList();

            var fullMessage = string.Join("; ", messages);

            return new FriendlyException(
                ApiErrorCode.BadRequest,
                fullMessage,
                fullMessage
            );
        }
    }
}
