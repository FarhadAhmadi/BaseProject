using BaseProject.Domain.Constants;
using BaseProject.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace BaseProject.Application.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public static class TransactionException
    {
        public static FriendlyException  TransactionNotCommitException()
            => throw new FriendlyException (ApiErrorCode.InternalServerError, ErrorMessage.TransactionNotCommit, ErrorMessage.TransactionNotCommit);

        public static FriendlyException  TransactionNotExecuteException(Exception ex)
            => throw new FriendlyException (ApiErrorCode.InternalServerError, ErrorMessage.TransactionNotExecute, ErrorMessage.TransactionNotExecute, ex);
    }
}
