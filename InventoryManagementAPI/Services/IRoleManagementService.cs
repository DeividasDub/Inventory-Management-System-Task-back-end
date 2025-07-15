using InventoryManagementAPI.DTOs;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public interface IRoleManagementService
    {
        Task<RoleResponseDto?> CreateRoleAsync(CreateRoleRequestDto request);
        Task<RoleResponseDto?> UpdateRoleAsync(string roleName, UpdateRoleRequestDto request);
        Task<bool> DeleteRoleAsync(string roleName);
        Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync();
    }
}
