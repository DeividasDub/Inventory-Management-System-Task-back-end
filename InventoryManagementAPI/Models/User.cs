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
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        public ICollection<UserRoleMapping> UserRoleMappings { get; set; } = new List<UserRoleMapping>();
    }
}
