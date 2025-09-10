using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Entities.Authorization;

namespace BaseProject.Application.Common.Interfaces
{
    /// <summary>
    /// Defines operations for managing permissions, actions, 
    /// role-based permissions, user-specific overrides, and authorization checks.
    /// </summary>
    public interface IPermissionService
    {
        #region Permission Records

        /// <summary>
        /// Gets a permission record by its unique identifier.
        /// </summary>
        Task<PermissionRecord?> GetPermissionRecordByIdAsync(string permissionId);

        /// <summary>
        /// Gets a permission record by its system name.
        /// </summary>
        Task<PermissionRecord?> GetPermissionRecordBySystemNameAsync(string systemName);

        /// <summary>
        /// Retrieves all available permission records.
        /// </summary>
        Task<List<PermissionRecord>> GetAllPermissionRecordsAsync();

        /// <summary>
        /// Inserts a new permission record.
        /// </summary>
        Task InsertPermissionRecordAsync(PermissionRecord permission);

        /// <summary>
        /// Updates an existing permission record.
        /// </summary>
        Task UpdatePermissionRecordAsync(PermissionRecord permission);

        /// <summary>
        /// Deletes a permission record.
        /// </summary>
        Task DeletePermissionRecordAsync(PermissionRecord permission);

        #endregion

        #region Authorization

        /// <summary>
        /// Checks whether a user (or the current user if null) 
        /// has the specified permission.
        /// </summary>
        Task<bool> AuthorizeAsync(string permissionSystemName, ApplicationUser? user = null);

        /// <summary>
        /// Checks whether a user (current user) 
        /// has the specified permission action.
        /// </summary>
        Task<bool> AuthorizeActionAsync(string permissionSystemName, string actionName);

        #endregion

        #region Permission Actions

        /// <summary>
        /// Gets all permission actions for a given permission system name and role.
        /// </summary>
        Task<IList<PermissionAction>> GetPermissionActionsAsync(string systemName, string roleId);

        /// <summary>
        /// Inserts a new permission action record.
        /// </summary>
        Task InsertPermissionActionRecordAsync(PermissionAction permissionAction);

        /// <summary>
        /// Deletes an existing permission action record.
        /// </summary>
        Task DeletePermissionActionRecordAsync(PermissionAction permissionAction);

        #endregion

        #region Role Permission Actions

        /// <summary>
        /// Gets all role-permission-action assignments for a given role.
        /// </summary>
        Task<IList<RolePermissionAction>> GetRolePermissionActionsAsync(string roleId);

        /// <summary>
        /// Gets a specific role-permission-action by role, permission, and action.
        /// </summary>
        Task<RolePermissionAction?> GetRolePermissionActionAsync(string roleId, string permissionSystemName, string actionName);

        /// <summary>
        /// Inserts a new role-permission-action assignment.
        /// </summary>
        Task InsertRolePermissionActionAsync(RolePermissionAction rolePermissionAction);

        /// <summary>
        /// Updates an existing role-permission-action assignment.
        /// </summary>
        Task UpdateRolePermissionActionAsync(RolePermissionAction rolePermissionAction);

        /// <summary>
        /// Deletes an existing role-permission-action assignment.
        /// </summary>
        Task DeleteRolePermissionActionAsync(RolePermissionAction rolePermissionAction);

        #endregion

        #region User Permission Actions

        /// <summary>
        /// Gets all user-permission-action overrides for a given user.
        /// </summary>
        Task<IList<UserPermissionAction>> GetUserPermissionActionsAsync(string userId);

        /// <summary>
        /// Gets a specific user-permission-action override by user, permission, and action.
        /// </summary>
        Task<UserPermissionAction?> GetUserPermissionActionAsync(string userId, string permissionSystemName, string actionName);

        /// <summary>
        /// Inserts a new user-permission-action override.
        /// </summary>
        Task InsertUserPermissionActionAsync(UserPermissionAction userPermissionAction);

        /// <summary>
        /// Updates an existing user-permission-action override.
        /// </summary>
        Task UpdateUserPermissionActionAsync(UserPermissionAction userPermissionAction);

        /// <summary>
        /// Deletes an existing user-permission-action override.
        /// </summary>
        Task DeleteUserPermissionActionAsync(UserPermissionAction userPermissionAction);

        #endregion
    }
}
