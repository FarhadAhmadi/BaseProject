using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Interfaces;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    public class UserRepository(ApplicationDbContext context, SqlDapperContext dapperContext) : GenericRepository<ApplicationUser>(context, dapperContext), IUserRepository { }

}
