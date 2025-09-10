using BaseProject.Domain.Entities.Security;
using BaseProject.Domain.Interfaces;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    public class PermissionActionRepository(ApplicationDbContext context, SqlDapperContext dapperContext) : GenericRepository<PermissionAction>(context, dapperContext), IPermissionActionRepository { }
}
