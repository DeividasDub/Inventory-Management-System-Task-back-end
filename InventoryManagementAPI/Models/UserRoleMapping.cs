namespace InventoryManagementAPI.Models
{
    public class UserRoleMapping
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public int RoleId { get; set; }
        public UserRole Role { get; set; } = null!;
    }
}
