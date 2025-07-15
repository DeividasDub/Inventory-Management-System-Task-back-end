using System.ComponentModel.DataAnnotations;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.DTOs
{
    public class UpdateUserRoleRequestDto
    {
        [Required]
        public UserRole Role { get; set; }
    }
}
