using InventoryManagementAPI.DTOs.Role;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public interface IRoleManagementService
    {
        Task<UserRole?> CreateRoleAsync(CreateRoleRequestDto request);
        Task<UserRole?> UpdateRoleAsync(int roleId, UpdateRoleRequestDto request);
        Task<bool> DeleteRoleAsync(int roleId);
        Task<IEnumerable<UserRole>> GetAllRolesAsync();
    }
}
