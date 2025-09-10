using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    public class UserRepository(
        ApplicationDbContext _context,
        SqlDapperContext _dapperContext
    ) : GenericRepository<ApplicationUser>(_context, _dapperContext), IUserRepository
    {
        public async Task<IEnumerable<ApplicationRole>> GetUserRolesAsync(string userId)
        {
            return await (from ur in _context.UserRoles
                    join r in _context.Roles on ur.RoleId equals r.Id
                    where ur.UserId == userId
                    select r)
                    .AsNoTracking()
                    .ToListAsync();
        }
    }
}
