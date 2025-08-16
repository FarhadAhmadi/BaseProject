using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.DTOs.AuthIdentity.UsersIdentity;
using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BaseProject.Application.Services
{
    public class UserService(
    IFileService storageService,
    UserManager<ApplicationUser> userManager,
    IUnitOfWork unitOfWork) : IUserService
    {
        private readonly IFileService _storageService = storageService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<List<UserResponseDto>> Get(CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Avatar)
                .ToListAsync(cancellationToken: cancellationToken);

            List<UserResponseDto> result = users.Select(x => new UserResponseDto
            {
                Id = x.Id,
                Email = x.Email,
                UserName = x.UserName,
                FullName = x.Name,
                Roles = x.UserRoles.Select(ur => ur.Role.Name).ToList(),
                Avatar = x.Avatar != null ? x.Avatar.PathMedia : null
            }).ToList();

            return result;
        }

        public async Task Update(UserUpdateRequestDto request, CancellationToken cancellationToken)
        {

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken)
                ?? throw AuthIdentityException.ThrowUserNotExist();

            user.Email = request.Email ?? user.Email;
            user.Name = request.Name ?? user.Name;

            //Luu Avatar vào Host
            if (request.MediaFile != null)
            {
                var thumb = await _unitOfWork.Media.GetFirstOrDefaultAsync<Media>(
                    filter: x => x.MediaId == user.AvatarId
                );

                //C?p nh?t Avatar
                if (thumb.PathMedia != null)
                {
                    await _storageService.DeleteFileAsync(new DTOs.File.DeleteFileRequestDto { FileName = thumb.PathMedia });
                }
                var pathMedia = await _storageService.AddFileAsync(request.MediaFile);

                thumb.FileSize = request.MediaFile.Length;
                thumb.PathMedia = pathMedia.Path;

                await _unitOfWork.ExecuteInTransactionAsync(() => _unitOfWork.Media.Update(thumb), cancellationToken);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                throw AuthIdentityException.ThrowUpdateUnsuccessful(errors);
            }
        }

        public async Task Delete(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw AuthIdentityException.ThrowAccountDoesNotExist();

            var avatar = await _unitOfWork.Media.GetFirstOrDefaultAsync<Media>(
                filter: x => x.MediaId == user.AvatarId
            );

            if (avatar != null)
            {
                if (avatar.PathMedia != null)
                    await _storageService.DeleteFileAsync(new DTOs.File.DeleteFileRequestDto { FileName = avatar.PathMedia });
                await _unitOfWork.ExecuteInTransactionAsync(() => _unitOfWork.Media.Delete(avatar), cancellationToken);
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                throw AuthIdentityException.ThrowDeleteUnsuccessful();
            }
        }
        // Gán quy?n ngu?i dùng và c?p nh?t scope
        public async Task RoleAssign(RoleAssignRequestDto request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId)
                ?? throw AuthIdentityException.ThrowAccountDoesNotExist();

            // Handle Role Removal
            var removedRoles = request.Roles.Where(x => !x.Selected).Select(x => x.Name).ToList();
            foreach (var roleName in removedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName))
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }

            // Handle Role Assignment
            var addedRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
            foreach (var roleName in addedRoles)
            {
                if (!await _userManager.IsInRoleAsync(user, roleName))
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            // Manage Scope Claims

            // Retrieve the existing scope claim
            var existingClaims = await _userManager.GetClaimsAsync(user);
            var scopeClaim = existingClaims.FirstOrDefault(c => c.Type == "scope");

            // Get scopes from the request
            var newScopes = request.Scopes.Where(x => x.Selected).Select(x => x.Name).ToList();

            if (scopeClaim != null)
            {
                // If there is an existing scope claim, remove it
                await _userManager.RemoveClaimAsync(user, scopeClaim);
            }

            if (newScopes.Any())
            {
                // Add the new scope claim
                var newScopeClaim = new Claim("scope", string.Join(" ", newScopes));
                await _userManager.AddClaimAsync(user, newScopeClaim);
            }
        }
    }
}
