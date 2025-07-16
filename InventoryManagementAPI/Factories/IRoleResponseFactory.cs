using InventoryManagementAPI.DTOs.Role;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public interface IRoleResponseFactory
    {
        RoleResponseDto CreateRoleResponse(UserRole role);
        IEnumerable<RoleResponseDto> CreateRoleResponses(IEnumerable<UserRole> roles);
    }
}
