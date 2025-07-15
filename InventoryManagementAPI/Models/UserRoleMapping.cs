namespace InventoryManagementAPI.Models
{
    public class UserRoleMapping
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string RoleName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = null!;
    }
}
