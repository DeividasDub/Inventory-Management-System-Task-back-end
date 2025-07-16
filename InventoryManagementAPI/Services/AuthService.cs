using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTOs.Auth;
using InventoryManagementAPI.Models;
using InventoryManagementAPI.Factories;

namespace InventoryManagementAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IAuthResponseFactory _authResponseFactory;

        public AuthService(ApplicationDbContext context, IJwtService jwtService, IConfiguration configuration, IAuthResponseFactory authResponseFactory)
        {
            _context = context;
            _jwtService = jwtService;
            _configuration = configuration;
            _authResponseFactory = authResponseFactory;
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
                RoleId = 2
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

            return _authResponseFactory.CreateAuthResponse(userWithRoles, token, DateTime.UtcNow.AddHours(expirationHours));
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

            return _authResponseFactory.CreateAuthResponse(user, token, DateTime.UtcNow.AddHours(expirationHours));
        }
    }
}
