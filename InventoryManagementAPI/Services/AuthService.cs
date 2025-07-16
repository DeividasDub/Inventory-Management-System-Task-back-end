using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTOs.Auth;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<(User?, string?)> RegisterAsync(RegisterRequestDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return (null, null);
            }

            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, 12),
                CreatedOn = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userRoleMapping = new UserRoleMapping
            {
                UserId = user.Id,
                RoleId = 2
            };
            _context.UserRoleMappings.Add(userRoleMapping);
            await _context.SaveChangesAsync();

            var userWithRoles = await _context.Users
                .Include(u => u.UserRoleMappings)
                .ThenInclude(urm => urm.Role)
                .FirstAsync(u => u.Id == user.Id);

            var token = _jwtService.GenerateToken(userWithRoles);

            return (userWithRoles, token);
        }

        public async Task<(User?, string?)> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .Include(u => u.UserRoleMappings)
                .ThenInclude(urm => urm.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return (null, null);
            }

            var token = _jwtService.GenerateToken(user);

            return (user, token);
        }
    }
}
