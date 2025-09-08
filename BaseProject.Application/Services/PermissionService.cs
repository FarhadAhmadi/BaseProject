using BaseProject.Application.Common.Interfaces;
using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Entities.Security;
using BaseProject.Domain.Interfaces;
using BaseProject.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Services
{
    /// <summary>
    /// Permission service
    /// </summary>
    public partial class PermissionService(
        ICurrentUser currentUser,
        ICacheBase cacheManager,
        IUnitOfWork unitOfWork
        ) : IPermissionService
    {

        #region Utilities

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="customerRole">Customer role</param>
        /// <returns>true - authorized; otherwise, false</returns>
        protected virtual async Task<bool> Authorize(string permissionRecordSystemName, ApplicationRole role)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            string key = string.Format(CacheKey.PERMISSIONS_ALLOWED_KEY, role.Id, permissionRecordSystemName);
            return await cacheManager.GetAsync(key, async () =>
            {
                var permissionRecord = await unitOfWork.PermissionRecordRepository.Table.FirstOrDefaultAsync(x => x.SystemName == permissionRecordSystemName);
                return permissionRecord?.UserRoles.Contains(role.Id) ?? false;
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual async Task DeletePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            unitOfWork.PermissionRecordRepository.Delete(permission);
            await unitOfWork.SaveChangesAsync();

            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="permissionId">Permission identifier</param>
        /// <returns>Permission</returns>
        public virtual Task<PermissionRecord> GetPermissionRecordById(string permissionId)
        {
            return unitOfWork.PermissionRecordRepository.GetByIdAsync(permissionId);
        }

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="systemName">Permission system name</param>
        /// <returns>Permission</returns>
        public virtual async Task<PermissionRecord> GetPermissionRecordBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return await Task.FromResult<PermissionRecord>(null);

            var query = from pr in unitOfWork.PermissionRecordRepository.Table
                        where pr.SystemName == systemName
                        orderby pr.Id
                        select pr;

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets all permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual async Task<IList<PermissionRecord>> GetAllPermissionRecords()
        {
            var query = from pr in unitOfWork.PermissionRecordRepository.Table
                        orderby pr.Name
                        select pr;
            return await query.ToListAsync();
        }

        /// <summary>
        /// Inserts a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual async Task InsertPermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            await unitOfWork.PermissionRecordRepository.AddAsync(permission);

            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Updates the permission
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual async Task UpdatePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            unitOfWork.PermissionRecordRepository.Update(permission);
            await unitOfWork.SaveChangesAsync();

            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual async Task<bool> Authorize(PermissionRecord permission)
        {
            return await Authorize(permission, await currentUser.GetCurrentUser());
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <param name="customer">Customer</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual async Task<bool> Authorize(PermissionRecord permission, ApplicationUser user)
        {
            if (permission == null)
                return false;

            if (user == null)
                return false;

            return await Authorize(permission.SystemName, user);
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual async Task<bool> Authorize(string permissionRecordSystemName)
        {
            return await Authorize(permissionRecordSystemName, await currentUser.GetCurrentUser());
        }

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="customer">Customer</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual async Task<bool> Authorize(string permissionRecordSystemName, ApplicationUser user)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var roles = await unitOfWork.Users.GetUserRolesAsync(user.Id);

            foreach (var role in roles)
                if (await Authorize(permissionRecordSystemName, role))
                    //yes, we have such permission
                    return true;

            //no permission found
            return false;
        }

        /// <summary>
        /// Gets a permission action
        /// </summary>
        /// <param name="systemName">Permission system name</param>
        /// <param name="customeroleId">Customer role ident</param>
        /// <returns>Permission action</returns>
        public virtual async Task<IList<PermissionAction>> GetPermissionActions(string systemName, string roleId)
        {
            return await unitOfWork.PermissionActionRepository.Table
                    .Where(x => x.SystemName == systemName && x.UserRoleId == roleId).ToListAsync();
        }

        /// <summary>
        /// Inserts a permission action record
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual async Task InsertPermissionActionRecord(PermissionAction permissionAction)
        {
            if (permissionAction == null)
                throw new ArgumentNullException("permissionAction");

            //insert
            await unitOfWork.PermissionActionRepository.AddAsync(permissionAction);
            await unitOfWork.SaveChangesAsync();
            //clear cache
            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Inserts a permission action record
        /// </summary>
        /// <param name="permission">Permission</param>
        public virtual async Task DeletePermissionActionRecord(PermissionAction permissionAction)
        {
            if (permissionAction == null)
                throw new ArgumentNullException("permissionAction");

            //delete
            unitOfWork.PermissionActionRepository.Delete(permissionAction);
            await unitOfWork.SaveChangesAsync();
            //clear cache
            await cacheManager.RemoveByPrefix(CacheKey.PERMISSIONS_PATTERN_KEY);
        }

        /// <summary>
        /// Authorize permission for action
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="permissionActionName">Permission action name</param>
        /// <returns>true - authorized; otherwise, false</returns>
        public virtual async Task<bool> AuthorizeAction(string permissionRecordSystemName, string permissionActionName)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName) || string.IsNullOrEmpty(permissionActionName))
                return false;

            if (!await Authorize(permissionRecordSystemName))
                return false;

            var userId = currentUser.GetCurrentUserId();

            var customerRoles = await unitOfWork.Users.GetUserRolesAsync(userId);
            foreach (var role in customerRoles.Where(x => x.Active))
            {
                if (!await Authorize(permissionRecordSystemName, role))
                    continue;

                var key = string.Format(CacheKey.PERMISSIONS_ALLOWED_ACTION_KEY, role.Id, permissionRecordSystemName, permissionActionName);
                var permissionAction = await cacheManager.GetAsync(key, async () =>
                {
                    return await unitOfWork.PermissionActionRepository.Table
                        .FirstOrDefaultAsync(x => x.SystemName == permissionRecordSystemName && x.UserRoleId == role.Id && x.Action == permissionActionName);
                });
                if (permissionAction != null)
                    return false;
            }

            return true;
        }

        #endregion
    }
}
