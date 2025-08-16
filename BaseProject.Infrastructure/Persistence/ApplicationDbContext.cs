using BaseProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BaseProject.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser, RoleIdentity, string,
        IdentityUserClaim<string>, UserRoles, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly AuditInterceptor _auditInterceptor;

        // Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditInterceptor auditInterceptor = null)
            : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ForgotPassword> ForgotPassword { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_auditInterceptor != null)
                optionsBuilder.AddInterceptors(_auditInterceptor);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply all configurations
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Identity table customizations
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin").HasKey(l => new
            {
                l.LoginProvider,
                l.ProviderKey,
                l.UserId
            });
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens").HasKey(x => x.UserId);

            // Seed data (if you have a Seed extension)
            builder.Seed();
        }
    }
}
