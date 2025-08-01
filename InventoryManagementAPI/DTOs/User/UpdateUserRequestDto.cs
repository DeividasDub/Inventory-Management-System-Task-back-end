using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.DTOs.User
{
    public class UpdateUserRequestDto
    {
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
        public int RoleId { get; set; }
    }
}
