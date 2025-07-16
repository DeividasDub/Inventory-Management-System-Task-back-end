using InventoryManagementAPI.DTOs.Auth;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public interface IAuthResponseFactory
    {
        AuthResponseDto CreateAuthResponse(User user, string token, DateTime expiresAt);
    }
}
