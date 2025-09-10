using BaseProject.Application.Common.Interfaces;
using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Entities.Authorization;
using BaseProject.Domain.Interfaces;
using BaseProject.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Services
{
    /// <inheritdoc/>
    public partial class PermissionService(
        ICurrentUser currentUser,
        ICacheBase cacheManager,
        IUnitOfWork unitOfWork
    ) : IPermissionService
    {
        #region Utilities
        private async Task<bool> AuthorizePermissionForRoleAsync(string permissionSystemName, ApplicationRole role)
        {
            if (string.IsNullOrWhiteSpace(permissionSystemName) || role == null)
                return false;

            string cacheKey = string.Format(CacheKey.PERMISSIONS_ALLOWED_KEY, role.Id, permissionSystemName);

            return await cacheManager.GetAsync(cacheKey, async () =>
            {
                return await unitOfWork.PermissionRecords.Table
                    .Where(pr => pr.SystemName == permissionSystemName)
                    .SelectMany(pr => pr.Actions)
                    .SelectMany(a => a.RolePermissionActions)
                    .AnyAsync(rpa => rpa.RoleId == role.Id);
            });
        }

        #endregion

        #region CRUD Methods
        public async Task DeletePermissionRecordAsync(PermissionRecord permission)
        {
            ArgumentNullException.ThrowIfNull(permission);

            unitOfWork.PermissionRecords.Delete(permission);
            await unitOfWork.SaveChangesAsync();
            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        public async Task<PermissionRecord?> GetPermissionRecordByIdAsync(string permissionId) 
            => await unitOfWork.PermissionRecords.GetByIdAsync(permissionId);

        public async Task<PermissionRecord?> GetPermissionRecordBySystemNameAsync(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName)) return null;

            return await unitOfWork.PermissionRecords.Table
                .FirstOrDefaultAsync(pr => pr.SystemName == systemName);
        }

        public async Task<List<PermissionRecord>> GetAllPermissionRecordsAsync() 
            => await unitOfWork.PermissionRecords.Table
                .OrderBy(pr => pr.Name)
                .ToListAsync();

        public async Task InsertPermissionRecordAsync(PermissionRecord permission)
        {
            ArgumentNullException.ThrowIfNull(permission);

            await unitOfWork.PermissionRecords.AddAsync(permission);
            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        public async Task UpdatePermissionRecordAsync(PermissionRecord permission)
        {
            ArgumentNullException.ThrowIfNull(permission);

            unitOfWork.PermissionRecords.Update(permission);
            await unitOfWork.SaveChangesAsync();
            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        #endregion

        #region Authorization Methods

        public async Task<bool> AuthorizeAsync(string permissionSystemName, ApplicationUser? user = null)
        {
            if (string.IsNullOrWhiteSpace(permissionSystemName))
                return false;

            user ??= await currentUser.GetCurrentUser();
            if (user == null) return false;

            // 1. Check user override (cached)
            string overrideCacheKey = string.Format(CacheKey.PERMISSIONS_USER_OVERRIDE_KEY, user.Id, permissionSystemName);

            bool? userOverride = await cacheManager.GetAsync(overrideCacheKey, async () =>
            {
                var records = await unitOfWork.UserPermissionActions.Table
                    .Include(upo => upo.PermissionAction)
                        .ThenInclude(pa => pa.PermissionRecord)
                    .Where(upo =>
                        upo.UserId == user.Id &&
                        upo.PermissionAction.PermissionRecord.SystemName == permissionSystemName)
                    .ToListAsync();

                if (records.Count == 0) return false; // no overrides

                // If any action is allowed, we allow the permission
                if (records.Any(r => r.IsAllowed)) return true;

                // All actions exist but none allowed => deny
                return false;
            });

            if (userOverride.HasValue)
                return userOverride.Value; // override wins

            // 2. Fallback: check role-based (already cached per role)
            IEnumerable<ApplicationRole> roles = await unitOfWork.Users.GetUserRolesAsync(user.Id);

            foreach (ApplicationRole role in roles)
            {
                if (await AuthorizePermissionForRoleAsync(permissionSystemName, role))
                    return true;
            }

            return false; // No role allowed
        }

        public async Task<bool> AuthorizeActionAsync(string permissionSystemName, string actionName)
        {
            if (string.IsNullOrWhiteSpace(permissionSystemName) || string.IsNullOrWhiteSpace(actionName))
                return false;

            ApplicationUser user = await currentUser.GetCurrentUser();
            if (user == null) return false;

            // 1. Check user override (cached)
            string overrideCacheKey = string.Format(CacheKey.PERMISSIONS_USER_OVERRIDE_ACTION_KEY, user.Id, permissionSystemName, actionName);

            bool? userOverride = await cacheManager.GetAsync(overrideCacheKey, async () =>
            {
                UserPermissionAction? record = await unitOfWork.UserPermissionActions.Table
                    .Include(upo => upo.PermissionAction)
                        .ThenInclude(pa => pa.PermissionRecord)
                    .FirstOrDefaultAsync(upo =>
                        upo.UserId == user.Id &&
                        upo.PermissionAction.PermissionRecord.SystemName == permissionSystemName &&
                        (upo.PermissionAction.Name == actionName || upo.PermissionAction.SystemName == actionName));

                return record?.IsAllowed; // null = no override
            });

            if (userOverride.HasValue)
                return userOverride.Value; // override wins

            // 2. Fallback: role-based check
            IEnumerable<ApplicationRole> roles = await unitOfWork.Users.GetUserRolesAsync(user.Id);

            PermissionRecord? permissionRecord = await unitOfWork.PermissionRecords.Table
                .Include(pr => pr.Actions)
                    .ThenInclude(a => a.RolePermissionActions)
                .FirstOrDefaultAsync(pr => pr.SystemName == permissionSystemName);

            if (permissionRecord == null) return false;

            foreach (ApplicationRole? role in roles.Where(r => r.Active))
            {
                string cacheKey = string.Format(CacheKey.PERMISSIONS_ALLOWED_ACTION_KEY, role.Id, permissionSystemName, actionName);

                PermissionAction? hasPermissionAction = await cacheManager.GetAsync(cacheKey, async () =>
                {
                    return permissionRecord.Actions
                        .SelectMany(a => a.RolePermissionActions)
                        .Where(rpa => rpa.RoleId == role.Id)
                        .Select(rpa => rpa.PermissionAction)
                        .FirstOrDefault(pa => pa.Name == actionName || pa.SystemName == actionName);
                });

                if (hasPermissionAction != null) return true;
            }

            return false;
        }

        #endregion

        #region PermissionAction CRUD

        public async Task<IList<PermissionAction>> GetPermissionActionsAsync(string systemName, string roleId) 
            => await unitOfWork.PermissionActions.Table
                .Where(x => x.SystemName == systemName && x.RolePermissionActions.Any(rp => rp.RoleId == roleId))
                .ToListAsync();

        public async Task InsertPermissionActionRecordAsync(PermissionAction permissionAction)
        {
            ArgumentNullException.ThrowIfNull(permissionAction);

            await unitOfWork.PermissionActions.AddAsync(permissionAction);
            await unitOfWork.SaveChangesAsync();
            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        public async Task DeletePermissionActionRecordAsync(PermissionAction permissionAction)
        {
            ArgumentNullException.ThrowIfNull(permissionAction);

            unitOfWork.PermissionActions.Delete(permissionAction);
            await unitOfWork.SaveChangesAsync();
            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        #endregion

        #region RolePermissionAction CRUD

        public async Task<IList<RolePermissionAction>> GetRolePermissionActionsAsync(string roleId)
        {
            ArgumentException.ThrowIfNullOrEmpty(roleId);

            return await unitOfWork.RolePermissionActions.Table
                .Include(rpa => rpa.PermissionAction)
                    .ThenInclude(pa => pa.PermissionRecord)
                .Where(rpa => rpa.RoleId == roleId)
                .ToListAsync();
        }

        public async Task<RolePermissionAction?> GetRolePermissionActionAsync(string roleId, string permissionSystemName, string actionName)
        {
            ArgumentException.ThrowIfNullOrEmpty(roleId);
            ArgumentException.ThrowIfNullOrEmpty(permissionSystemName);
            ArgumentException.ThrowIfNullOrEmpty(actionName);

            return await unitOfWork.RolePermissionActions.Table
                .Include(rpa => rpa.PermissionAction)
                    .ThenInclude(pa => pa.PermissionRecord)
                .FirstOrDefaultAsync(rpa =>
                    rpa.RoleId == roleId &&
                    rpa.PermissionAction.PermissionRecord.SystemName == permissionSystemName &&
                    (rpa.PermissionAction.SystemName == actionName || rpa.PermissionAction.Name == actionName));
        }

        public async Task InsertRolePermissionActionAsync(RolePermissionAction rolePermissionAction)
        {
            ArgumentNullException.ThrowIfNull(rolePermissionAction);

            await unitOfWork.RolePermissionActions.AddAsync(rolePermissionAction);
            await unitOfWork.SaveChangesAsync();

            await InvalidateRolePermissionCache(rolePermissionAction.RoleId, rolePermissionAction.PermissionAction);
        }

        public async Task UpdateRolePermissionActionAsync(RolePermissionAction rolePermissionAction)
        {
            ArgumentNullException.ThrowIfNull(rolePermissionAction);

            unitOfWork.RolePermissionActions.Update(rolePermissionAction);
            await unitOfWork.SaveChangesAsync();

            await InvalidateRolePermissionCache(rolePermissionAction.RoleId, rolePermissionAction.PermissionAction);
        }

        public async Task DeleteRolePermissionActionAsync(RolePermissionAction rolePermissionAction)
        {
            ArgumentNullException.ThrowIfNull(rolePermissionAction);

            unitOfWork.RolePermissionActions.Delete(rolePermissionAction);
            await unitOfWork.SaveChangesAsync();

            await InvalidateRolePermissionCache(rolePermissionAction.RoleId, rolePermissionAction.PermissionAction);
        }

        /// <summary>
        /// Clears cache keys related to a role’s permission actions.
        /// </summary>
        private async Task InvalidateRolePermissionCache(string roleId, PermissionAction? permissionAction)
        {
            if (permissionAction == null) return;

            // Record-level cache
            string recordKey = string.Format(CacheKey.PERMISSIONS_ALLOWED_KEY, roleId, permissionAction.PermissionRecord.SystemName);
            await cacheManager.RemoveAsync(recordKey);

            // Action-level cache
            string actionKey = string.Format(CacheKey.PERMISSIONS_ALLOWED_ACTION_KEY, roleId, permissionAction.PermissionRecord.SystemName, permissionAction.SystemName);
            await cacheManager.RemoveAsync(actionKey);
        }

        #endregion

        #region UserPermissionAction CRUD

        public async Task<IList<UserPermissionAction>> GetUserPermissionActionsAsync(string userId)
        {
            ArgumentException.ThrowIfNullOrEmpty(userId);

            return await unitOfWork.UserPermissionActions.Table
                .Include(upa => upa.PermissionAction)
                    .ThenInclude(pa => pa.PermissionRecord)
                .Where(upa => upa.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserPermissionAction?> GetUserPermissionActionAsync(string userId, string permissionSystemName, string actionName)
        {
            ArgumentException.ThrowIfNullOrEmpty(userId);
            ArgumentException.ThrowIfNullOrEmpty(permissionSystemName);
            ArgumentException.ThrowIfNullOrEmpty(actionName);

            return await unitOfWork.UserPermissionActions.Table
                .Include(upa => upa.PermissionAction)
                    .ThenInclude(pa => pa.PermissionRecord)
                .FirstOrDefaultAsync(upa =>
                    upa.UserId == userId &&
                    upa.PermissionAction.PermissionRecord.SystemName == permissionSystemName &&
                    (upa.PermissionAction.SystemName == actionName || upa.PermissionAction.Name == actionName));
        }

        public async Task InsertUserPermissionActionAsync(UserPermissionAction userPermissionAction)
        {
            ArgumentNullException.ThrowIfNull(userPermissionAction);

            await unitOfWork.UserPermissionActions.AddAsync(userPermissionAction);
            await unitOfWork.SaveChangesAsync();

            // Clear relevant cache keys for this user
            await InvalidateUserPermissionCache(userPermissionAction.UserId, userPermissionAction.PermissionAction);
        }

        public async Task UpdateUserPermissionActionAsync(UserPermissionAction userPermissionAction)
        {
            ArgumentNullException.ThrowIfNull(userPermissionAction);

            unitOfWork.UserPermissionActions.Update(userPermissionAction);
            await unitOfWork.SaveChangesAsync();

            // Clear relevant cache keys for this user
            await InvalidateUserPermissionCache(userPermissionAction.UserId, userPermissionAction.PermissionAction);
        }

        public async Task DeleteUserPermissionActionAsync(UserPermissionAction userPermissionAction)
        {
            ArgumentNullException.ThrowIfNull(userPermissionAction);

            unitOfWork.UserPermissionActions.Delete(userPermissionAction);
            await unitOfWork.SaveChangesAsync();

            // Clear relevant cache keys for this user
            await InvalidateUserPermissionCache(userPermissionAction.UserId, userPermissionAction.PermissionAction);
        }

        /// <summary>
        /// Clears cache keys related to a specific user's overrides.
        /// </summary>
        private async Task InvalidateUserPermissionCache(string userId, PermissionAction? permissionAction)
        {
            if (permissionAction == null) return;

            // Invalidate record-level cache
            string recordKey = string.Format(CacheKey.PERMISSIONS_USER_OVERRIDE_KEY, userId, permissionAction.PermissionRecordId);
            await cacheManager.RemoveAsync(recordKey);

            // Invalidate action-level cache
            string actionKey = string.Format(CacheKey.PERMISSIONS_USER_OVERRIDE_ACTION_KEY, userId, permissionAction.PermissionRecord.SystemName, permissionAction.SystemName);
            await cacheManager.RemoveAsync(actionKey);
        }

        #endregion
    }
}
