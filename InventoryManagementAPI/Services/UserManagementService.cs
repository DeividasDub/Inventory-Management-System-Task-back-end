using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTOs.User;
using InventoryManagementAPI.Models;
using InventoryManagementAPI.Factories;

namespace InventoryManagementAPI.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserResponseFactory _userResponseFactory;

        public UserManagementService(ApplicationDbContext context, IUserResponseFactory userResponseFactory)
        {
            _context = context;
            _userResponseFactory = userResponseFactory;
        }

        public async Task<UserResponseDto?> CreateUserAsync(CreateUserRequestDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return null;
            }

            var role = await _context.UserRoles.FirstOrDefaultAsync(r => r.Id == request.RoleId);
            if (role == null)
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
                RoleId = role.Id
            };
            _context.UserRoleMappings.Add(userRoleMapping);
            await _context.SaveChangesAsync();

            return _userResponseFactory.CreateUserResponse(user, role.Name);
        }

        public async Task<UserResponseDto?> UpdateUserRoleAsync(int userId, int roleId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoleMappings)
                .ThenInclude(urm => urm.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user == null)
            {
                return null;
            }

            var role = await _context.UserRoles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null)
            {
                return null;
            }

            _context.UserRoleMappings.RemoveRange(user.UserRoleMappings);
            
            var userRoleMapping = new UserRoleMapping
            {
                UserId = userId,
                RoleId = role.Id
            };
            _context.UserRoleMappings.Add(userRoleMapping);
            await _context.SaveChangesAsync();

            return _userResponseFactory.CreateUserResponse(user, role.Name);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.UserRoleMappings)
                .ThenInclude(urm => urm.Role)
                .ToListAsync();

            return _userResponseFactory.CreateUserResponses(users);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
