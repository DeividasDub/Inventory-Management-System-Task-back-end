using InventoryManagementAPI.DTOs.Auth;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public interface IAuthService
    {
        Task<(User?, string?)> RegisterAsync(RegisterRequestDto request);
        Task<(User?, string?)> LoginAsync(LoginRequestDto request);
    }
}
