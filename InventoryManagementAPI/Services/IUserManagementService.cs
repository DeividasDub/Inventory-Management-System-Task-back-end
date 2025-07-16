using InventoryManagementAPI.DTOs.User;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public interface IUserManagementService
    {
        Task<User?> CreateUserAsync(CreateUserRequestDto request);
        Task<User?> UpdateUserRoleAsync(int userId, int roleId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(int userId);
    }
}
