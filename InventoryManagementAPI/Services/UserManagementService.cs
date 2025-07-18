using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTOs.User;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly ApplicationDbContext _context;

        public UserManagementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> CreateUserAsync(CreateUserRequestDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email && !u.Deleted))
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

            user.UserRoleMappings = new List<UserRoleMapping> { userRoleMapping };
            userRoleMapping.Role = role;

            return user;
        }

        public async Task<User?> UpdateUserRoleAsync(int userId, int roleId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoleMappings)
                .ThenInclude(urm => urm.Role)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.Deleted);
            
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

            user.UserRoleMappings = new List<UserRoleMapping> { userRoleMapping };
            userRoleMapping.Role = role;

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.UserRoleMappings)
                .ThenInclude(urm => urm.Role)
                .Where(u => !u.Deleted)
                .ToListAsync();

            return users;
        }

        public async Task<User?> UpdateUserAsync(int userId, UpdateUserRequestDto request)
        {
            var user = await _context.Users
                .Include(u => u.UserRoleMappings)
                .ThenInclude(urm => urm.Role)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.Deleted);
            
            if (user == null)
            {
                return null;
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email && u.Id != userId && !u.Deleted))
            {
                return null;
            }

            var role = await _context.UserRoles.FirstOrDefaultAsync(r => r.Id == request.RoleId);
            if (role == null)
            {
                return null;
            }

            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            if (!user.UserRoleMappings.Any(urm => urm.RoleId == request.RoleId))
            {
                _context.UserRoleMappings.RemoveRange(user.UserRoleMappings);
                
                var userRoleMapping = new UserRoleMapping
                {
                    UserId = userId,
                    RoleId = role.Id
                };
                _context.UserRoleMappings.Add(userRoleMapping);
                
                user.UserRoleMappings = new List<UserRoleMapping> { userRoleMapping };
                userRoleMapping.Role = role;
            }

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null || user.Deleted)
            {
                return false;
            }

            user.Deleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
