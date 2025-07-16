using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTOs.Supplier;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext _context;

        public SupplierService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Supplier?> CreateSupplierAsync(CreateSupplierRequestDto request)
        {
            if (await _context.Suppliers.AnyAsync(s => s.Email == request.Email))
            {
                return null;
            }

            var supplier = new Supplier
            {
                Name = request.Name,
                ContactName = request.ContactName,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address,
                CreatedOn = DateTime.UtcNow
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return supplier;
        }

        public async Task<Supplier?> UpdateSupplierAsync(int id, UpdateSupplierRequestDto request)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return null;
            }

            if (await _context.Suppliers.AnyAsync(s => s.Email == request.Email && s.Id != id))
            {
                return null;
            }

            supplier.Name = request.Name;
            supplier.ContactName = request.ContactName;
            supplier.Phone = request.Phone;
            supplier.Email = request.Email;
            supplier.Address = request.Address;

            await _context.SaveChangesAsync();

            return supplier;
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return false;
            }

            var hasProducts = await _context.Products.AnyAsync(p => p.SupplierId == id);
            if (hasProducts)
            {
                return false;
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            return supplier;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            var suppliers = await _context.Suppliers.ToListAsync();
            return suppliers;
        }
    }
}
