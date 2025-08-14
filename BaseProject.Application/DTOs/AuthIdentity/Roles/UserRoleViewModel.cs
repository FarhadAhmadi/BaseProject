using BaseProject.Application.DTOs.AuthIdentity.UsersIdentity;

namespace BaseProject.Application.DTOs.AuthIdentity.Roles;

public class UserRoleViewModel
{
    public UserUpdateRequestDto EditUser { get; set; }

    public RoleAssignRequestDto EditRole { get; set; }
}
