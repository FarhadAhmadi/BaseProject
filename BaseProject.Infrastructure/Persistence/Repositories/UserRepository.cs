using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    public class UserRepository(ApplicationDbContext context, SqlDapperContext dapperContext) : GenericRepository<User>(context, dapperContext), IUserRepository { }

}
