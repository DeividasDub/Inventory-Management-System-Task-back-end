using InventoryManagementAPI.DTOs.Auth;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public interface IAuthModelFactory
    {
        AuthResponseDto PrepareAuthResponseModel(User user, string token, DateTime expiresAt);
    }
}
