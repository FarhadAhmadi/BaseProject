using BaseProject.Application.Common.Interfaces;
using System.Diagnostics;

namespace BaseProject.Application.Common.Extensions.PredefinedLogs
{
    public static class SecurityLogExtensions
    {
        private static string GetTraceId() => Activity.Current?.Id ?? "N/A";
        public static void LogAccessDenied(this IAppLogger logger, string userId, string resource)
        {
            logger.Warning("Access denied | UserId: {UserId} | Resource: {Resource} | TraceId: {TraceId}", userId, resource, GetTraceId());
        }

        public static void LogRoleChange(this IAppLogger logger, string userId, string oldRole, string newRole)
        {
            logger.Info("Role changed | UserId: {UserId} | OldRole: {OldRole} | NewRole: {NewRole} | TraceId: {TraceId}",
                userId, oldRole, newRole, GetTraceId());
        }

        public static void LogPermissionChange(this IAppLogger logger, string userId, string permission, bool granted)
        {
            logger.Info("Permission {Action} | UserId: {UserId} | Permission: {Permission} | TraceId: {TraceId}",
                granted ? "granted" : "revoked", userId, permission, GetTraceId());
        }
    }
}
