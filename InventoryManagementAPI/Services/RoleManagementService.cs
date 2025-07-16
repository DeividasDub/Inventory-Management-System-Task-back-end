using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTOs.Role;
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

        public async Task<UserRole?> CreateRoleAsync(CreateRoleRequestDto request)
        {
            if (await _context.UserRoles.AnyAsync(r => r.Name == request.Name))
            {
                return null;
            }

            var role = new UserRole
            {
                Name = request.Name,
                Description = request.Description,
                CreatedOn = DateTime.UtcNow
            };

            _context.UserRoles.Add(role);
            await _context.SaveChangesAsync();

            return role;
        }

        public async Task<UserRole?> UpdateRoleAsync(int roleId, UpdateRoleRequestDto request)
        {
            var role = await _context.UserRoles.FindAsync(roleId);
            
            if (role == null)
            {
                return null;
            }

            if (await _context.UserRoles.AnyAsync(r => r.Name == request.Name && r.Id != roleId))
            {
                return null;
            }

            role.Name = request.Name;
            role.Description = request.Description;

            await _context.SaveChangesAsync();

            return role;
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            var role = await _context.UserRoles.FindAsync(roleId);
            if (role == null)
            {
                return false;
            }

            var hasUsers = await _context.UserRoleMappings.AnyAsync(urm => urm.RoleId == roleId);
            if (hasUsers)
            {
                return false;
            }

            _context.UserRoles.Remove(role);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<UserRole>> GetAllRolesAsync()
        {
            var roles = await _context.UserRoles.ToListAsync();

            return roles;
        }
    }
}
