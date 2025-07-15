using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTOs;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IJwtService jwtService, IConfiguration configuration)
        {
            _context = context;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return null;
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
                RoleName = "Staff"
            };
            _context.UserRoleMappings.Add(userRoleMapping);
            await _context.SaveChangesAsync();

            var userWithRoles = await _context.Users
                .Include(u => u.UserRoleMappings)
                .ThenInclude(urm => urm.Role)
                .FirstAsync(u => u.Id == user.Id);

            var token = _jwtService.GenerateToken(userWithRoles);
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "24");

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Roles = userWithRoles.UserRoleMappings.Select(urm => urm.Role.Name).ToList(),
                ExpiresAt = DateTime.UtcNow.AddHours(expirationHours)
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .Include(u => u.UserRoleMappings)
                .ThenInclude(urm => urm.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null;
            }

            var token = _jwtService.GenerateToken(user);
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "24");

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Roles = user.UserRoleMappings.Select(urm => urm.Role.Name).ToList(),
                ExpiresAt = DateTime.UtcNow.AddHours(expirationHours)
            };
        }
    }
}
