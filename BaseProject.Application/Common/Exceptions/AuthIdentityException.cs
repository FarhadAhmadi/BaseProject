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
            => throw new FriendlyException (ErrorCode.NotFound, AuthIdentityErrorMessage.TokenNotExistMessage, AuthIdentityErrorMessage.TokenNotExistMessage);

        public static FriendlyException  ThrowTokenNotActive()
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.TokenNotActiveMessage, AuthIdentityErrorMessage.TokenNotActiveMessage);

        public static FriendlyException  ThrowAccountDoesNotExist()
            => throw new FriendlyException (ErrorCode.NotFound, AuthIdentityErrorMessage.AccountDoesNotExistMessage, AuthIdentityErrorMessage.AccountDoesNotExistMessage);

        public static FriendlyException  ThrowLoginUnsuccessful()
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.LoginUnsuccessfulMessage, AuthIdentityErrorMessage.LoginUnsuccessfulMessage);

        public static FriendlyException  ThrowUsernameAvailable()
            => throw new FriendlyException (ErrorCode.NotFound, AuthIdentityErrorMessage.UsernameAvailableMessage, AuthIdentityErrorMessage.UsernameAvailableMessage);

        public static FriendlyException  ThrowEmailAvailable()
            => throw new FriendlyException (ErrorCode.NotFound, AuthIdentityErrorMessage.EmailAvailableMessage, AuthIdentityErrorMessage.EmailAvailableMessage);

        public static FriendlyException  ThrowRegisterUnsuccessful(string errors)
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.RegisterUnsuccessfulMessage, errors);

        public static FriendlyException  ThrowUserNotExist()
            => throw new FriendlyException (ErrorCode.NotFound, AuthIdentityErrorMessage.UserNotExistMessage, AuthIdentityErrorMessage.UserNotExistMessage);

        public static FriendlyException  ThrowUpdateUnsuccessful(string errors)
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.UpdateUnsuccessfulMessage, errors);

        public static FriendlyException  ThrowDeleteUnsuccessful()
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.DeleteUnsuccessfulMessage, AuthIdentityErrorMessage.DeleteUnsuccessfulMessage);

        public static FriendlyException  ThrowInvalidFacebookToken()
            => throw new FriendlyException (ErrorCode.NotFound, AuthIdentityErrorMessage.InvalidFacebookTokenMessage, AuthIdentityErrorMessage.InvalidFacebookTokenMessage);

        public static FriendlyException  ThrowErrorLinkedFacebook()
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.ErrorLinkedFacebookMessage, AuthIdentityErrorMessage.ErrorLinkedFacebookMessage);

        public static FriendlyException  ThrowRegisterFacebookUnsuccessful(string errors)
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.RegisterFacebookUnsuccessfulMessage, errors);

        public static FriendlyException  ThrowInvalidGoogleToken()
            => throw new FriendlyException (ErrorCode.NotFound, AuthIdentityErrorMessage.InvalidGoogleTokenMessage, AuthIdentityErrorMessage.InvalidGoogleTokenMessage);

        public static FriendlyException  ThrowErrorLinkedGoogle()
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.ErrorLinkedGoogleMessage, AuthIdentityErrorMessage.ErrorLinkedGoogleMessage);

        public static FriendlyException  ThrowRegisterGoogleUnsuccessful(string errors)
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.RegisterGoogleUnsuccessfulMessage, errors);

        public static FriendlyException  ThrowInvalidAppleToken()
            => throw new FriendlyException (ErrorCode.NotFound, AuthIdentityErrorMessage.InvalidAppleTokenMessage, AuthIdentityErrorMessage.InvalidAppleTokenMessage);

        public static FriendlyException  ThrowEmailRequired()
            => throw new FriendlyException (ErrorCode.NotFound, AuthIdentityErrorMessage.EmailRequiredMessage, AuthIdentityErrorMessage.EmailRequiredMessage);

        public static FriendlyException  ThrowErrorLinkedApple()
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.ErrorLinkedAppleMessage, AuthIdentityErrorMessage.ErrorLinkedAppleMessage);

        public static FriendlyException  ThrowRegisterAppleUnsuccessful(string errors)
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.RegisterAppleUnsuccessfulMessage, errors);

        public static FriendlyException  ThrowUserNotFound()
            => throw new FriendlyException (ErrorCode.NotFound, AuthIdentityErrorMessage.UserNotFoundMessage, AuthIdentityErrorMessage.UserNotFoundMessage);

        public static FriendlyException  ThrowGenerateTheNewOTP()
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.GenerateTheNewOTPMessage, AuthIdentityErrorMessage.GenerateTheNewOTPMessage);

        public static FriendlyException  ThrowOTPWrong()
            => throw new FriendlyException (ErrorCode.BadRequest, AuthIdentityErrorMessage.OTPWrongMessage, AuthIdentityErrorMessage.OTPWrongMessage);
    }
}
