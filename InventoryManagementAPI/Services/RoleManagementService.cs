using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTOs;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly ApplicationDbContext _context;

        public RoleManagementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RoleResponseDto?> CreateRoleAsync(CreateRoleRequestDto request)
        {
            if (await _context.UserRoles.AnyAsync(r => r.Name == request.Name))
            {
                return null;
            }

            var role = new UserRole
            {
                Name = request.Name
            };

            _context.UserRoles.Add(role);
            await _context.SaveChangesAsync();

            return new RoleResponseDto
            {
                Name = role.Name
            };
        }

        public async Task<RoleResponseDto?> UpdateRoleAsync(string roleName, UpdateRoleRequestDto request)
        {
            var role = await _context.UserRoles.FindAsync(roleName);
            
            if (role == null)
            {
                return null;
            }

            if (await _context.UserRoles.AnyAsync(r => r.Name == request.Name && r.Name != roleName))
            {
                return null;
            }

            role.Name = request.Name;
            await _context.SaveChangesAsync();

            return new RoleResponseDto
            {
                Name = role.Name
            };
        }

        public async Task<bool> DeleteRoleAsync(string roleName)
        {
            var role = await _context.UserRoles.FindAsync(roleName);
            
            if (role == null)
            {
                return false;
            }

            var hasUsers = await _context.UserRoleMappings.AnyAsync(urm => urm.RoleName == roleName);
            if (hasUsers)
            {
                return false;
            }

            _context.UserRoles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync()
        {
            var roles = await _context.UserRoles
                .Select(r => new RoleResponseDto
                {
                    Name = r.Name
                })
                .ToListAsync();

            return roles;
        }

    }
}
