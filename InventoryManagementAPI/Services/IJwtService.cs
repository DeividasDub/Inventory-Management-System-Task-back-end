using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        bool ValidateToken(string token);
    }
}
