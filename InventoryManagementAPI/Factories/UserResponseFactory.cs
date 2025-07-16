using InventoryManagementAPI.DTOs.User;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public class UserModelFactory : IUserModelFactory
    {
        public UserResponseDto PrepareUserResponseModel(User user, string roleName)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleNames = new List<string> { roleName },
                CreatedOn = user.CreatedOn
            };
        }

        public UserResponseDto PrepareUserResponseModel(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleNames = user.UserRoleMappings?.Select(urm => urm.Role.Name).ToList() ?? new List<string>(),
                CreatedOn = user.CreatedOn
            };
        }

        public IEnumerable<UserResponseDto> PrepareUserListResponseModel(IEnumerable<User> users)
        {
            return users.Select(PrepareUserResponseModel);
        }
    }
}
