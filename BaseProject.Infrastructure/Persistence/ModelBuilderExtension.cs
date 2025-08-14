using BaseProject.Domain.Entities;
using BaseProject.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Infrastructure.Persistence
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            var roleId = new string("A3314BE5-4C77-4FB6-82AD-302014682A73");

            var adminId = new string("69DB714F-9576-45BA-B5B7-F00649BE01DE");

            modelBuilder.Entity<RoleIdentity>().HasData(new RoleIdentity
            {
                Id = roleId,
                Name = Role.Admin.ToString(),
                NormalizedName = Role.Admin.ToString(),
            });
            modelBuilder.Entity<RoleIdentity>().HasData(new RoleIdentity
            {
                Id = new string("B4314BE5-4C77-4FB6-82AD-302014682B13"),
                Name = Role.User.ToString(),
                NormalizedName = Role.User.ToString(),
            });

            var hashed = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hashed.HashPassword(null, "P@ssw0rd"),
                SecurityStamp = string.Empty,
                Name = "Admin 1",
            });

            modelBuilder.Entity<UserRoles>().HasData(new UserRoles
            {
                RoleId = roleId,
                UserId = adminId
            });
        }
    }
}
