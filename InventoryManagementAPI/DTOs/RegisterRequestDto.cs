using System.ComponentModel.DataAnnotations;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        public UserRole Role { get; set; }
    }
}
