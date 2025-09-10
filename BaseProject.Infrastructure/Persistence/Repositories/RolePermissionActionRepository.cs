using BaseProject.Domain.Entities.Authorization;
using BaseProject.Domain.Interfaces;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    public class RolePermissionActionRepository(ApplicationDbContext context, SqlDapperContext dapperContext) : GenericRepository<RolePermissionAction>(context, dapperContext), IRolePermissionActionRepository { }
}
