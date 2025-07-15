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
            if (await _context.Roles.AnyAsync(r => r.Name == request.Name))
            {
                return null;
            }

            var role = new Role
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedAt = role.CreatedAt
            };
        }

        public async Task<RoleResponseDto?> UpdateRoleAsync(int roleId, UpdateRoleRequestDto request)
        {
            var role = await _context.Roles.FindAsync(roleId);
            
            if (role == null)
            {
                return null;
            }

            if (await _context.Roles.AnyAsync(r => r.Name == request.Name && r.Id != roleId))
            {
                return null;
            }

            role.Name = request.Name;
            role.Description = request.Description;
            await _context.SaveChangesAsync();

            return new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedAt = role.CreatedAt
            };
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            
            if (role == null)
            {
                return false;
            }

            var hasUsers = await _context.Users.AnyAsync(u => u.RoleId == roleId);
            if (hasUsers)
            {
                return false;
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync()
        {
            var roles = await _context.Roles
                .Select(r => new RoleResponseDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return roles;
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }
    }
}
