using BaseProject.Application.Common.Interfaces;
using System.Diagnostics;

namespace BaseProject.Application.Common.Extensions.PredefinedLogs
{
    public static class ErrorLogExtensions
    {
        private static string GetTraceId() => Activity.Current?.Id ?? "N/A";
        public static void LogHandledException(this IAppLogger logger, Exception ex, string context)
        {
            logger.Error(ex, "Handled exception in {Context} | TraceId: {TraceId}", context, GetTraceId());
        }

        public static void LogUnhandledException(this IAppLogger logger, Exception ex)
        {
            logger.Error(ex, "Unhandled exception | TraceId: {TraceId}", GetTraceId());
        }

        public static void LogWarning(this IAppLogger logger, string message)
        {
            logger.Warning("{Message} | TraceId: {TraceId}", message, GetTraceId());
        }
    }
}
