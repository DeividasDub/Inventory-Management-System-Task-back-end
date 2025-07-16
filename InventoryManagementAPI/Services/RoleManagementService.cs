using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTOs.Role;
using InventoryManagementAPI.Models;
using InventoryManagementAPI.Factories;

namespace InventoryManagementAPI.Services
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoleResponseFactory _roleResponseFactory;

        public RoleManagementService(ApplicationDbContext context, IRoleResponseFactory roleResponseFactory)
        {
            _context = context;
            _roleResponseFactory = roleResponseFactory;
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

            return _roleResponseFactory.CreateRoleResponse(role);
        }

        public async Task<RoleResponseDto?> UpdateRoleAsync(int roleId, UpdateRoleRequestDto request)
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
            await _context.SaveChangesAsync();

            return _roleResponseFactory.CreateRoleResponse(role);
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

        public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync()
        {
            var roles = await _context.UserRoles.ToListAsync();

            return _roleResponseFactory.CreateRoleResponses(roles);
        }

    }
}
