using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
