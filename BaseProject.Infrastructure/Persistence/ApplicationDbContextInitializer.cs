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
            var usersToAdd = new List<User>
            {
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
                }
            };

            foreach (var user in usersToAdd)
            {
                var exists = await _context.Users
                    .AnyAsync(u => u.UserName == user.UserName || u.Email == user.Email);

                if (!exists)
                {
                    await _context.Users.AddAsync(user);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
