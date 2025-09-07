using BaseProject.Domain.Entities.Security;
using BaseProject.Domain.Interfaces;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    public class PermissionRecordRepository(ApplicationDbContext context, SqlDapperContext dapperContext) : GenericRepository<PermissionRecord>(context, dapperContext), IPermissionRecordRepository { }
}
