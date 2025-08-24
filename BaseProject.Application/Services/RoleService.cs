using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.DTOs.AuthIdentity.Roles;
using BaseProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Application.Services
{
    public class RoleService(RoleManager<ApplicationRole> roleManager) : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

        public async Task<List<RoleResposneDto>> GetAll()
        {
            var roles = await _roleManager.Roles
                .Select(x => new RoleResposneDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();

            return roles;
        }
    }
}
