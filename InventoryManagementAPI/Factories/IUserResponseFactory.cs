using InventoryManagementAPI.DTOs.User;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public interface IUserModelFactory
    {
        UserResponseDto PrepareUserResponseModel(User user, string roleName);
        UserResponseDto PrepareUserResponseModel(User user);
        IEnumerable<UserResponseDto> PrepareUserListResponseModel(IEnumerable<User> users);
    }
}
