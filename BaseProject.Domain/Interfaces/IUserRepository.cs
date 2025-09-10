using BaseProject.Domain.Entities.Auth;

namespace BaseProject.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser> 
    {
        Task<IEnumerable<ApplicationRole>> GetUserRolesAsync(string userId);
    }
}
