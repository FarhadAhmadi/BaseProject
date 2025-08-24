using BaseProject.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BaseProject.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser, ApplicationRole, string,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>
    {
        private readonly AuditInterceptor _auditInterceptor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditInterceptor auditInterceptor = null)
            : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }

        // DbSets
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<UserPasswordReset> UserPasswordResets { get; set; }
        public DbSet<UserMedia> UserMedias { get; set; }
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

            // Identity table customizations with schema
            builder.Entity<ApplicationUser>().ToTable("Users", "Identity");
            builder.Entity<ApplicationRole>().ToTable("Roles", "Identity");
            builder.Entity<ApplicationUserRole>().ToTable("UserRoles", "Identity");
            builder.Entity<ApplicationUserClaim>().ToTable("UserClaims", "Identity");
            builder.Entity<ApplicationRoleClaim>().ToTable("RoleClaims", "Identity");

            builder.Entity<ApplicationUserLogin>().ToTable("UserLogins", "Identity")
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });

            builder.Entity<ApplicationUserToken>().ToTable("UserTokens", "Identity")
                .HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

            // Seed data
            builder.Seed();
        }
    }
}
