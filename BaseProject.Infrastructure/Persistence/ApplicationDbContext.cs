using BaseProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
                IdentityDbContext<ApplicationUser, RoleIdentity, string,
                IdentityUserClaim<string>, UserRoles, IdentityUserLogin<string>,
                IdentityRoleClaim<string>, IdentityUserToken<string>>(options)
    {
        //public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ForgotPassword> ForgotPassword { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin").HasKey(l => new
            {
                l.LoginProvider,
                l.ProviderKey,
                l.UserId
            });
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens").HasKey(x => x.UserId);

            builder.Seed();
        }
    }
}
