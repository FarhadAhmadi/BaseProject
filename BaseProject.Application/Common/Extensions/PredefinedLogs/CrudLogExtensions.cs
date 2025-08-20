using BaseProject.Application.Common.Interfaces;
using System.Diagnostics;

namespace BaseProject.Application.Common.Extensions.PredefinedLogs
{
    public static class CrudLogExtensions
    {
        private static string GetTraceId() => Activity.Current?.Id ?? "N/A";
        public static void LogCreate<T>(this IAppLogger logger, string entityId, string userId)
        {
            logger.Info("Created entity {Entity} | EntityId: {EntityId} | UserId: {UserId} | TraceId: {TraceId}",
                typeof(T).Name, entityId, userId, GetTraceId());
        }

        public static void LogUpdate<T>(this IAppLogger logger, string entityId, string userId)
        {
            logger.Info("Updated entity {Entity} | EntityId: {EntityId} | UserId: {UserId} | TraceId: {TraceId}",
                typeof(T).Name, entityId, userId, GetTraceId());
        }

        public static void LogDelete<T>(this IAppLogger logger, string entityId, string userId)
        {
            logger.Info("Deleted entity {Entity} | EntityId: {EntityId} | UserId: {UserId} | TraceId: {TraceId}",
                typeof(T).Name, entityId, userId, GetTraceId());
        }

        public static void LogRead<T>(this IAppLogger logger, string entityId, string userId)
        {
            logger.Debug("Read entity {Entity} | EntityId: {EntityId} | UserId: {UserId} | TraceId: {TraceId}",
                typeof(T).Name, entityId, userId, GetTraceId());
        }
    }
}
