using InventoryManagementAPI.DTOs;

namespace InventoryManagementAPI.Services
{
    public interface IUserManagementService
    {
        Task<UserResponseDto?> CreateUserAsync(CreateUserRequestDto request);
        Task<UserResponseDto?> UpdateUserRoleAsync(int userId, int roleId);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(int userId);
    }
}
