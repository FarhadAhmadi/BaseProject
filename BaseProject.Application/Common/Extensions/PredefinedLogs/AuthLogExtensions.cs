using BaseProject.Application.Common.Interfaces;
using System.Diagnostics;

namespace BaseProject.Application.Common.Extensions.PredefinedLogs
{
    public static class AuthLogExtensions
    {
        private static string GetTraceId() => Activity.Current?.Id ?? "N/A";

        public static void LogLoginAttempt(this IAppLogger logger, string userName)
        {
            logger.Info("Login attempt started | UserName: {UserName} | TraceId: {TraceId}",
                userName, GetTraceId());
        }

        public static void LogLoginInvalidUserName(this IAppLogger logger, string userName)
        {
            logger.Warning("Login failed | Reason: User not found | UserName: {UserName} | TraceId: {TraceId}",
                userName, GetTraceId());
        }

        public static void LogLoginInvalidPassword(this IAppLogger logger, string userId)
        {
            logger.Warning("Login failed | Reason: Invalid password | UserId: {UserId} | TraceId: {TraceId}",
                userId, GetTraceId());
        }

        public static void LogLoginResult(this IAppLogger logger, string userId, bool success)
        {
            logger.Info("Login {Result} | UserId: {UserId} | TraceId: {TraceId}",
                success ? "Successful" : "Failed", userId, GetTraceId());
        }

        public static void LogLogout(this IAppLogger logger, string userId)
        {
            logger.Info("User logged out | UserId: {UserId} | TraceId: {TraceId}", userId, GetTraceId());
        }

        public static void LogTokenRefresh(this IAppLogger logger, string userId)
        {
            logger.Debug("Token refreshed successfully | UserId: {UserId} | TraceId: {TraceId}", userId, GetTraceId());
        }

        public static void LogSignUpAttempt(this IAppLogger logger, string userName, string email)
        {
            logger.Debug("Sign-up attempt started | UserName: {UserName} | Email: {Email} | TraceId: {TraceId}",
                userName, email, GetTraceId());
        }

        public static void LogSignUpResult(this IAppLogger logger, string userName, bool success)
        {
            logger.Info("Sign-up {Result} | UserName: {UserName} | TraceId: {TraceId}",
                success ? "Successful" : "Failed", userName, GetTraceId());
        }
    }
}
