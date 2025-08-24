using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository(ApplicationDbContext context, SqlDapperContext dapperContext) : GenericRepository<UserRefreshToken>(context, dapperContext), IRefreshTokenRepository { }
}
