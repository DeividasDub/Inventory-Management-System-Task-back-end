using InventoryManagementAPI.DTOs.Supplier;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public interface ISupplierService
    {
        Task<Supplier?> CreateSupplierAsync(CreateSupplierRequestDto request);
        Task<Supplier?> UpdateSupplierAsync(int id, UpdateSupplierRequestDto request);
        Task<bool> DeleteSupplierAsync(int id);
        Task<Supplier?> GetSupplierByIdAsync(int id);
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
    }
}
