using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    public class ForgotPasswordRepository(ApplicationDbContext context, SqlDapperContext dapperContext) : GenericRepository<UserPasswordReset>(context, dapperContext), IForgotPasswordRepository { }
}
