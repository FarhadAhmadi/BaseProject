using BaseProject.Domain.Entities;
using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Entities.Authorization;
using BaseProject.Domain.Entities.Base;
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
        public DbSet<GenericAttribute> GenericAttributes { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<UserPasswordReset> UserPasswordResets { get; set; }
        public DbSet<UserMedia> UserMedias { get; set; }
        public DbSet<DefaultPermissionRecord> DefaultPermissionRecords { get; set; }
        public DbSet<PermissionRecord> PermissionRecords { get; set; }
        public DbSet<PermissionAction> PermissionActions { get; set; }
        public DbSet<RolePermissionAction> RolePermissionActions { get; set; }
        public DbSet<UserPermissionAction> UserPermissionActions { get; set; }

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

            // Seed data
            builder.Seed();
        }
    }
}
