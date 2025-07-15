using InventoryManagementAPI.DTOs;

namespace InventoryManagementAPI.Services
{
    public interface ISupplierService
    {
        Task<SupplierResponseDto?> CreateSupplierAsync(CreateSupplierRequestDto request);
        Task<SupplierResponseDto?> UpdateSupplierAsync(int id, UpdateSupplierRequestDto request);
        Task<bool> DeleteSupplierAsync(int id);
        Task<SupplierResponseDto?> GetSupplierByIdAsync(int id);
        Task<IEnumerable<SupplierResponseDto>> GetAllSuppliersAsync();
    }
}
