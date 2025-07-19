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
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
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
                entity.Property(e => e.Deleted).IsRequired().HasDefaultValue(false);
            });

            modelBuilder.Entity<UserRoleMapping>(entity =>
            {
                entity.HasKey(urm => new { urm.UserId, urm.RoleId });
                
                entity.HasOne(urm => urm.User)
                    .WithMany(u => u.UserRoleMappings)
                    .HasForeignKey(urm => urm.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(urm => urm.Role)
                    .WithMany(r => r.UserRoleMappings)
                    .HasForeignKey(urm => urm.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 1, Name = "Admin" },
                new UserRole { Id = 2, Name = "Staff" }
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
                },
                new User
                {
                    Id = 2,
                    Email = "staff1@inventory.com",
                    FirstName = "Staff",
                    LastName = "User",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Staff1", 12),
                    CreatedOn = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<UserRoleMapping>().HasData(
                new UserRoleMapping { UserId = 1, RoleId = 1 },
                new UserRoleMapping { UserId = 2, RoleId = 2 }
            );

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ContactName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CreatedOn).IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.SKU).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.SKU).IsRequired().HasMaxLength(100);
                entity.Property(e => e.QuantityInStock).IsRequired();
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedOn).IsRequired();
                
                entity.HasOne(p => p.Supplier)
                    .WithMany(s => s.Products)
                    .HasForeignKey(p => p.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StockMovement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Reason).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Date).IsRequired();
                
                entity.HasOne(sm => sm.Product)
                    .WithMany(p => p.StockMovements)
                    .HasForeignKey(sm => sm.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(sm => sm.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(sm => sm.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
