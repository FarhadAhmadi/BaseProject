using BaseProject.Application.DTOs.Common.AuthIdentity.UsersIdentity;

namespace BaseProject.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> Get(CancellationToken cancellationToken);
        Task Update(UserUpdateRequestDto request, CancellationToken cancellationToken);
        Task Delete(string userId, CancellationToken cancellationToken);
        Task RoleAssign(RoleAssignRequestDto request, CancellationToken cancellationToken);
    }

}
