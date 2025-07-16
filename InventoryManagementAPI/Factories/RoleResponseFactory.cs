using InventoryManagementAPI.DTOs.Role;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public class RoleResponseFactory : IRoleResponseFactory
    {
        public RoleResponseDto CreateRoleResponse(UserRole role)
        {
            return new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public IEnumerable<RoleResponseDto> CreateRoleResponses(IEnumerable<UserRole> roles)
        {
            return roles.Select(CreateRoleResponse);
        }
    }
}
