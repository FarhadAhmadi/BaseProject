using BaseProject.Application.Common.Utilities;
using BaseProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BaseProject.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer(ApplicationDbContext context, ILoggerFactory logger)
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger _logger = logger.CreateLogger<ApplicationDbContextInitializer>();

        public async Task InitializeAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
                await SeedUser();
            }
            catch (Exception exception)
            {
                _logger.LogError("Migration error {exception}", exception);
                throw;
            }
        }

        public async Task SeedUser()
        {
            await _context.Users.AddRangeAsync(
            new List<User>{
                new User
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    Password = "P@ssw0rd".Hash(),
                    Role = Domain.Enums.Role.Admin
                },
                new User
                {
                    UserName = "user",
                    Email = "user@gmail.com",
                    Password = "P@ssw0rd".Hash(),
                    Role = Domain.Enums.Role.User
                },
                }
            );
            await _context.SaveChangesAsync();
        }
    }
}
