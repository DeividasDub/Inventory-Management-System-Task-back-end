using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTOs;
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

        public async Task<UserResponseDto?> CreateUserAsync(CreateUserRequestDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return null;
            }

            var role = await _context.UserRoles.FirstOrDefaultAsync(r => r.Name == request.RoleName);
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
                RoleName = role.Name
            };
            _context.UserRoleMappings.Add(userRoleMapping);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleNames = new List<string> { role.Name },
                CreatedOn = user.CreatedOn
            };
        }

        public async Task<UserResponseDto?> UpdateUserRoleAsync(int userId, string roleName)
        {
            var user = await _context.Users
                .Include(u => u.UserRoleMappings)
                .ThenInclude(urm => urm.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user == null)
            {
                return null;
            }

            var role = await _context.UserRoles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                return null;
            }

            _context.UserRoleMappings.RemoveRange(user.UserRoleMappings);
            
            var userRoleMapping = new UserRoleMapping
            {
                UserId = userId,
                RoleName = role.Name
            };
            _context.UserRoleMappings.Add(userRoleMapping);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleNames = new List<string> { role.Name },
                CreatedOn = user.CreatedOn
            };
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.UserRoleMappings)
                .ThenInclude(urm => urm.Role)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    RoleNames = u.UserRoleMappings.Select(urm => urm.Role.Name).ToList(),
                    CreatedOn = u.CreatedOn
                })
                .ToListAsync();

            return users;
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
