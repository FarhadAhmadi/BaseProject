using BaseProject.Domain.Constants;
using BaseProject.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace BaseProject.Application.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public static class ProgramException
    {
        public static FriendlyException AppsettingNotSetException()
            => new(ApiErrorCode.InternalServerError, ErrorMessage.AppConfigurationMessage, ErrorMessage.InternalServerError);
    }
}
