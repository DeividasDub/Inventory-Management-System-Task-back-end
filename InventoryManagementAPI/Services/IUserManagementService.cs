using InventoryManagementAPI.DTOs;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public interface IUserManagementService
    {
        Task<UserResponseDto?> CreateUserAsync(CreateUserRequestDto request);
        Task<UserResponseDto?> UpdateUserRoleAsync(int userId, UserRole newRole);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(int userId);
    }
}
