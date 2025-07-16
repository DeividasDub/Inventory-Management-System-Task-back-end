using InventoryManagementAPI.DTOs.Role;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public interface IRoleModelFactory
    {
        RoleResponseDto PrepareRoleResponseModel(UserRole role);
        IEnumerable<RoleResponseDto> PrepareRoleListResponseModel(IEnumerable<UserRole> roles);
    }
}
