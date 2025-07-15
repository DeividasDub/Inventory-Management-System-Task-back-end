using InventoryManagementAPI.DTOs;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public interface IRoleManagementService
    {
        Task<RoleResponseDto?> CreateRoleAsync(CreateRoleRequestDto request);
        Task<RoleResponseDto?> UpdateRoleAsync(int roleId, UpdateRoleRequestDto request);
        Task<bool> DeleteRoleAsync(int roleId);
        Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync();
    }
}
