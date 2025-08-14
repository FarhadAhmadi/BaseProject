using BaseProject.Application.DTOs.AuthIdentity.Roles;

namespace BaseProject.Application.Common.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleResposneDto>> GetAll();
    }
}
