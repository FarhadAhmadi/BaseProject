using BaseProject.Application.Common.Interfaces;
using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Entities.Authorization;
using BaseProject.Domain.Interfaces;
using BaseProject.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Services
{
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
                PermissionRecord? permission = await unitOfWork.PermissionRecordRepository.Table
                    .Include(pr => pr.Actions)
                        .ThenInclude(a => a.RolePermissionActions)
                    .FirstOrDefaultAsync(pr => pr.SystemName == permissionSystemName);

                return permission?.Actions
                    .SelectMany(a => a.RolePermissionActions)
                    .Any(rpa => rpa.RoleId == role.Id) ?? false;
            });
        }

        #endregion

        #region CRUD Methods

        public async Task DeletePermissionRecordAsync(PermissionRecord permission)
        {
            ArgumentNullException.ThrowIfNull(permission);

            unitOfWork.PermissionRecordRepository.Delete(permission);
            await unitOfWork.SaveChangesAsync();
            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        public async Task<PermissionRecord?> GetPermissionRecordByIdAsync(string permissionId) 
            => await unitOfWork.PermissionRecordRepository.GetByIdAsync(permissionId);

        public async Task<PermissionRecord?> GetPermissionRecordBySystemNameAsync(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName)) return null;

            return await unitOfWork.PermissionRecordRepository.Table
                .FirstOrDefaultAsync(pr => pr.SystemName == systemName);
        }

        public async Task<List<PermissionRecord>> GetAllPermissionRecordsAsync() 
            => await unitOfWork.PermissionRecordRepository.Table
                .OrderBy(pr => pr.Name)
                .ToListAsync();

        public async Task InsertPermissionRecordAsync(PermissionRecord permission)
        {
            ArgumentNullException.ThrowIfNull(permission);

            await unitOfWork.PermissionRecordRepository.AddAsync(permission);
            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        public async Task UpdatePermissionRecordAsync(PermissionRecord permission)
        {
            ArgumentNullException.ThrowIfNull(permission);

            unitOfWork.PermissionRecordRepository.Update(permission);
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

            IEnumerable<ApplicationRole> roles = await unitOfWork.Users.GetUserRolesAsync(user.Id);

            PermissionRecord? permissionRecord = await unitOfWork.PermissionRecordRepository.Table
                .Include(pr => pr.Actions)
                    .ThenInclude(a => a.RolePermissionActions)
                .FirstOrDefaultAsync(pr => pr.SystemName == permissionSystemName);

            if (permissionRecord == null) return false;

            foreach (ApplicationRole? role in roles.Where(r => r.Active))
            {
                bool hasPermissionRecord = permissionRecord.Actions
                    .SelectMany(a => a.RolePermissionActions)
                    .Any(rpa => rpa.RoleId == role.Id);

                if (!hasPermissionRecord) continue;

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
            => await unitOfWork.PermissionActionRepository.Table
                .Where(x => x.SystemName == systemName && x.RolePermissionActions.Any(rp => rp.RoleId == roleId))
                .ToListAsync();

        public async Task InsertPermissionActionRecordAsync(PermissionAction permissionAction)
        {
            ArgumentNullException.ThrowIfNull(permissionAction);

            await unitOfWork.PermissionActionRepository.AddAsync(permissionAction);
            await unitOfWork.SaveChangesAsync();
            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        public async Task DeletePermissionActionRecordAsync(PermissionAction permissionAction)
        {
            ArgumentNullException.ThrowIfNull(permissionAction);

            unitOfWork.PermissionActionRepository.Delete(permissionAction);
            await unitOfWork.SaveChangesAsync();
            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        #endregion
    }
}
