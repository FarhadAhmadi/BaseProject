using BaseProject.Application.Common.Utilities;
using BaseProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BaseProject.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public ApplicationDbContextInitializer(ApplicationDbContext context, ILoggerFactory logger)
        {
            _context = context;
            _logger = logger.CreateLogger<ApplicationDbContextInitializer>();
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError("Migration error {exception}", exception);
                throw;
            }
        }
    }
}
