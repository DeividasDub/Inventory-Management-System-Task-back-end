using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Models
{
    public class UserRole
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        public ICollection<UserRoleMapping> UserRoleMappings { get; set; } = new List<UserRoleMapping>();
    }
}
