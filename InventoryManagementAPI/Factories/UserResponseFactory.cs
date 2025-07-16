using InventoryManagementAPI.DTOs.User;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public class UserResponseFactory : IUserResponseFactory
    {
        public UserResponseDto CreateUserResponse(User user, IEnumerable<string> roleNames)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleNames = roleNames.ToList(),
                CreatedOn = user.CreatedOn
            };
        }

        public UserResponseDto CreateUserResponse(User user, string roleName)
        {
            return CreateUserResponse(user, new List<string> { roleName });
        }

        public IEnumerable<UserResponseDto> CreateUserResponses(IEnumerable<User> users)
        {
            return users.Select(u => CreateUserResponse(u, u.UserRoleMappings.Select(urm => urm.Role.Name)));
        }
    }
}
