using BaseProject.Application.Common.Utilities;
using BaseProject.Domain.Entities.Auth;
using BaseProject.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Infrastructure.Persistence
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var roleId = new string("A3314BE5-4C77-4FB6-82AD-302014682A73");

            var adminId = new string("69DB714F-9576-45BA-B5B7-F00649BE01DE");

            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Id = roleId,
                Name = Role.Admin.ToString(),
                NormalizedName = Role.Admin.ToString(),
            });
            modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Id = new string("B4314BE5-4C77-4FB6-82AD-302014682B13"),
                Name = Role.User.ToString(),
                NormalizedName = Role.User.ToString(),
            });

            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "P@ssw0rd".HashPassword(),
                SecurityStamp = string.Empty,
                FullName = "Admin 1",
            });

            modelBuilder.Entity<ApplicationUserRole>().HasData(new ApplicationUserRole
            {
                RoleId = roleId,
                UserId = adminId
            });
        }
    }
}
