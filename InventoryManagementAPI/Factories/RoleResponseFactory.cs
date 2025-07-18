using InventoryManagementAPI.DTOs.Role;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public class RoleModelFactory : IRoleModelFactory
    {
        public RoleResponseDto PrepareRoleResponseModel(UserRole role)
        {
            return new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public IEnumerable<RoleResponseDto> PrepareRoleListResponseModel(IEnumerable<UserRole> roles)
        {
            return roles.Select(PrepareRoleResponseModel);
        }
    }
}
