using BaseProject.Domain.Constants;
using BaseProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Application.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public static class AuthIdentityException
    {
        public static FriendlyException  ThrowTokenNotExist()
            => throw new FriendlyException (ApiErrorCode.NotFound, AuthIdentityErrorMessage.TokenNotExistMessage, AuthIdentityErrorMessage.TokenNotExistMessage);

        public static FriendlyException  ThrowTokenNotActive()
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.TokenNotActiveMessage, AuthIdentityErrorMessage.TokenNotActiveMessage);

        public static FriendlyException  ThrowAccountDoesNotExist()
            => throw new FriendlyException (ApiErrorCode.NotFound, AuthIdentityErrorMessage.AccountDoesNotExistMessage, AuthIdentityErrorMessage.AccountDoesNotExistMessage);

        public static FriendlyException  ThrowLoginUnsuccessful()
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.LoginUnsuccessfulMessage, AuthIdentityErrorMessage.LoginUnsuccessfulMessage);

        public static FriendlyException  ThrowUsernameAvailable()
            => throw new FriendlyException (ApiErrorCode.NotFound, AuthIdentityErrorMessage.UsernameAvailableMessage, AuthIdentityErrorMessage.UsernameAvailableMessage);

        public static FriendlyException  ThrowEmailAvailable()
            => throw new FriendlyException (ApiErrorCode.NotFound, AuthIdentityErrorMessage.EmailAvailableMessage, AuthIdentityErrorMessage.EmailAvailableMessage);

        public static FriendlyException  ThrowRegisterUnsuccessful(string errors)
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.RegisterUnsuccessfulMessage, errors);

        public static FriendlyException  ThrowUserNotExist()
            => throw new FriendlyException (ApiErrorCode.NotFound, AuthIdentityErrorMessage.UserNotExistMessage, AuthIdentityErrorMessage.UserNotExistMessage);

        public static FriendlyException  ThrowUpdateUnsuccessful(string errors)
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.UpdateUnsuccessfulMessage, errors);

        public static FriendlyException  ThrowDeleteUnsuccessful()
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.DeleteUnsuccessfulMessage, AuthIdentityErrorMessage.DeleteUnsuccessfulMessage);

        public static FriendlyException  ThrowInvalidFacebookToken()
            => throw new FriendlyException (ApiErrorCode.NotFound, AuthIdentityErrorMessage.InvalidFacebookTokenMessage, AuthIdentityErrorMessage.InvalidFacebookTokenMessage);

        public static FriendlyException  ThrowErrorLinkedFacebook()
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.ErrorLinkedFacebookMessage, AuthIdentityErrorMessage.ErrorLinkedFacebookMessage);

        public static FriendlyException  ThrowRegisterFacebookUnsuccessful(string errors)
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.RegisterFacebookUnsuccessfulMessage, errors);

        public static FriendlyException  ThrowInvalidGoogleToken()
            => throw new FriendlyException (ApiErrorCode.NotFound, AuthIdentityErrorMessage.InvalidGoogleTokenMessage, AuthIdentityErrorMessage.InvalidGoogleTokenMessage);

        public static FriendlyException  ThrowErrorLinkedGoogle()
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.ErrorLinkedGoogleMessage, AuthIdentityErrorMessage.ErrorLinkedGoogleMessage);

        public static FriendlyException  ThrowRegisterGoogleUnsuccessful(string errors)
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.RegisterGoogleUnsuccessfulMessage, errors);

        public static FriendlyException  ThrowInvalidAppleToken()
            => throw new FriendlyException (ApiErrorCode.NotFound, AuthIdentityErrorMessage.InvalidAppleTokenMessage, AuthIdentityErrorMessage.InvalidAppleTokenMessage);

        public static FriendlyException  ThrowEmailRequired()
            => throw new FriendlyException (ApiErrorCode.NotFound, AuthIdentityErrorMessage.EmailRequiredMessage, AuthIdentityErrorMessage.EmailRequiredMessage);

        public static FriendlyException  ThrowErrorLinkedApple()
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.ErrorLinkedAppleMessage, AuthIdentityErrorMessage.ErrorLinkedAppleMessage);

        public static FriendlyException  ThrowRegisterAppleUnsuccessful(string errors)
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.RegisterAppleUnsuccessfulMessage, errors);

        public static FriendlyException  ThrowUserNotFound()
            => throw new FriendlyException (ApiErrorCode.NotFound, AuthIdentityErrorMessage.UserNotFoundMessage, AuthIdentityErrorMessage.UserNotFoundMessage);

        public static FriendlyException  ThrowGenerateTheNewOTP()
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.GenerateTheNewOTPMessage, AuthIdentityErrorMessage.GenerateTheNewOTPMessage);

        public static FriendlyException  ThrowOTPWrong()
            => throw new FriendlyException (ApiErrorCode.BadRequest, AuthIdentityErrorMessage.OTPWrongMessage, AuthIdentityErrorMessage.OTPWrongMessage);
    }
}
