using BaseProject.Application.Common.Interfaces;
using System.Diagnostics;

namespace BaseProject.Application.Common.Extensions.PredefinedLogs
{
    public static class AuthLogExtensions
    {
        private static string GetTraceId() => Activity.Current?.Id ?? "N/A";
        public static void LogLoginAttempt(this IAppLogger logger, string userName)
        {
            logger.Info("Login attempt | UserName: {UserName} | TraceId: {TraceId}", userName, GetTraceId());
        }

        public static void LogLoginResult(this IAppLogger logger, string userId, bool success)
        {
            logger.Info("Login {Result} | UserId: {UserId} | TraceId: {TraceId}",
                success ? "successful" : "failed", userId, GetTraceId());
        }

        public static void LogLogout(this IAppLogger logger, string userId)
        {
            logger.Info("Logout | UserId: {UserId} | TraceId: {TraceId}", userId, GetTraceId());
        }

        public static void LogTokenRefresh(this IAppLogger logger, string userId)
        {
            logger.Debug("Token refreshed | UserId: {UserId} | TraceId: {TraceId}", userId, GetTraceId());
        }

        public static void LogSignUpAttempt(this IAppLogger logger, string userName, string email)
        {
            logger.Debug("SignUp attempt started for {UserName} | Email: {Email}", userName, email);
        }

        public static void LogSignUpResult(this IAppLogger logger, string userName, bool success)
        {
            logger.Info("Sign Up {Result} | UserName: {userName} | TraceId: {TraceId}",
                success ? "successful" : "failed", userName, GetTraceId());
        }
    }
}
