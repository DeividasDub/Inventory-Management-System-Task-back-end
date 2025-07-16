using InventoryManagementAPI.DTOs.Supplier;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public interface ISupplierModelFactory
    {
        SupplierResponseDto PrepareSupplierResponseModel(Supplier supplier);
        IEnumerable<SupplierResponseDto> PrepareSupplierListResponseModel(IEnumerable<Supplier> suppliers);
    }
}
