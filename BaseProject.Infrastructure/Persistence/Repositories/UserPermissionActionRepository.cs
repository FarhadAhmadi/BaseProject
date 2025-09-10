using BaseProject.Domain.Entities.Authorization;
using BaseProject.Domain.Interfaces;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    public class UserPermissionActionRepository(ApplicationDbContext context, SqlDapperContext dapperContext) : GenericRepository<UserPermissionAction>(context, dapperContext), IUserPermissionActionRepository { }
}
