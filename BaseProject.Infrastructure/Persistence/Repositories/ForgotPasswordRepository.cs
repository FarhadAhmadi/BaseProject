using BaseProject.Domain.Entities;
using BaseProject.Domain.Interfaces;
using System.Linq;

namespace BaseProject.Infrastructure.Persistence.Repositories
{
    public class ForgotPasswordRepository(ApplicationDbContext context, SqlDapperContext dapperContext) : GenericRepository<UserPasswordReset>(context, dapperContext), IForgotPasswordRepository { }
}
