using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Entities.Authorization;

namespace BaseProject.Application.Common.Interfaces
{
    /// <summary>
    /// Permission service interface
    /// </summary>
    public interface IPermissionService
    {
        #region Permission Records

        Task<PermissionRecord?> GetPermissionRecordByIdAsync(string permissionId);
        Task<PermissionRecord?> GetPermissionRecordBySystemNameAsync(string systemName);
        Task<List<PermissionRecord>> GetAllPermissionRecordsAsync();
        Task InsertPermissionRecordAsync(PermissionRecord permission);
        Task UpdatePermissionRecordAsync(PermissionRecord permission);
        Task DeletePermissionRecordAsync(PermissionRecord permission);

        #endregion

        #region Authorization

        /// <summary>
        /// Checks if a user has the specified permission.
        /// </summary>
        /// <param name="permissionSystemName">Permission system name</param>
        /// <param name="user">Optional user, if null will use current user</param>
        Task<bool> AuthorizeAsync(string permissionSystemName, ApplicationUser? user = null);

        /// <summary>
        /// Checks if a user has the specified permission action.
        /// </summary>
        /// <param name="permissionSystemName">Permission system name</param>
        /// <param name="actionName">Action name</param>
        Task<bool> AuthorizeActionAsync(string permissionSystemName, string actionName);

        #endregion

        #region Permission Actions

        Task<IList<PermissionAction>> GetPermissionActionsAsync(string systemName, string roleId);
        Task InsertPermissionActionRecordAsync(PermissionAction permissionAction);
        Task DeletePermissionActionRecordAsync(PermissionAction permissionAction);

        #endregion
    }
}
