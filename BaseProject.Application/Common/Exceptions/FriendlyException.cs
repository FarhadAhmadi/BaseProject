using BaseProject.Domain.Enums;

namespace BaseProject.Application.Common.Exceptions
{
    public class FriendlyException : Exception
    {
        public string UserFriendlyMessage { get; set; }
        public ErrorCode ErrorCode { get; set; }

        public FriendlyException(ErrorCode errorCode, string userFriendlyMessage, Exception? innerException = null)
            : base(userFriendlyMessage, innerException)
        {
            ErrorCode = errorCode;
            UserFriendlyMessage = userFriendlyMessage;
        }

        public FriendlyException(string message, string userFriendlyMessage, Exception? innerException = null)
            : base(message, innerException)
        {
            UserFriendlyMessage = userFriendlyMessage;
        }

        public FriendlyException(ErrorCode errorCode, string message, string userFriendlyMessage, Exception? innerException = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            UserFriendlyMessage = userFriendlyMessage;
        }
    }


}
