using InventoryManagementAPI.DTOs.User;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public interface IUserResponseFactory
    {
        UserResponseDto CreateUserResponse(User user, IEnumerable<string> roleNames);
        UserResponseDto CreateUserResponse(User user, string roleName);
        IEnumerable<UserResponseDto> CreateUserResponses(IEnumerable<User> users);
    }
}
