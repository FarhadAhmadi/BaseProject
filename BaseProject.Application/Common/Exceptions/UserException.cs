using BaseProject.Domain.Constants;
using BaseProject.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace BaseProject.Application.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public static class UserException
    {
        public static FriendlyException  UserAlreadyExistsException(string field)
            => new(ApiErrorCode.BadRequest, string.Format(UserErrorMessage.AlreadyExists, field), string.Format(UserErrorMessage.AlreadyExists, field));

        public static FriendlyException  UserUnauthorizedException()
            => new(ApiErrorCode.Unauthorized, UserErrorMessage.Unauthorized, UserErrorMessage.Unauthorized);

        public static FriendlyException  InternalException(Exception? exception)
            => new(ApiErrorCode.InternalServerError, ErrorMessage.InternalServerError, ErrorMessage.InternalServerError, exception);

        public static FriendlyException  BadRequestException(string errorMessage)
            => new(ApiErrorCode.BadRequest, errorMessage, errorMessage);

        public static FriendlyException  EmailFormatException(string errorMessage)
            => new(ApiErrorCode.BadRequest, errorMessage, errorMessage);
    }
}
