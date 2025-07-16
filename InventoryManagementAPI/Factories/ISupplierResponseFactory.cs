using InventoryManagementAPI.DTOs.Supplier;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public interface ISupplierResponseFactory
    {
        SupplierResponseDto CreateSupplierResponse(Supplier supplier);
        IEnumerable<SupplierResponseDto> CreateSupplierResponses(IEnumerable<Supplier> suppliers);
    }
}
