using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    public class MediaRepository(ApplicationDbContext context, SqlDapperContext dapperContext) : GenericRepository<UserMedia>(context, dapperContext), IMediaRepository { }

}
