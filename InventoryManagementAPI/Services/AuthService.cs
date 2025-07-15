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
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, 12),
                RoleId = 2,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _context.Entry(user).Reference(u => u.Role).LoadAsync();

            var token = _jwtService.GenerateToken(user);
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "24");

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role.Name,
                ExpiresAt = DateTime.UtcNow.AddHours(expirationHours)
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == request.Email);
            
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
                Role = user.Role.Name,
                ExpiresAt = DateTime.UtcNow.AddHours(expirationHours)
            };
        }
    }
}
