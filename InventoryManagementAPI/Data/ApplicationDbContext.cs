using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Name);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.CreatedOn).IsRequired();
            });

            modelBuilder.Entity<UserRoleMapping>(entity =>
            {
                entity.HasKey(urm => new { urm.UserId, urm.RoleName });
                
                entity.HasOne(urm => urm.User)
                    .WithMany(u => u.UserRoleMappings)
                    .HasForeignKey(urm => urm.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(urm => urm.Role)
                    .WithMany(r => r.UserRoleMappings)
                    .HasForeignKey(urm => urm.RoleName)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Name = "Admin" },
                new UserRole { Name = "Staff" }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@inventory.com",
                    FirstName = "Admin",
                    LastName = "User",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!", 12),
                    CreatedOn = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<UserRoleMapping>().HasData(
                new UserRoleMapping { UserId = 1, RoleName = "Admin" }
            );
        }
    }
}
