using InventoryManagementAPI.DTOs.Auth;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public class AuthResponseFactory : IAuthResponseFactory
    {
        public AuthResponseDto CreateAuthResponse(User user, string token, DateTime expiresAt)
        {
            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Roles = user.UserRoleMappings.Select(urm => urm.Role.Name).ToList(),
                ExpiresAt = expiresAt
            };
        }
    }
}
