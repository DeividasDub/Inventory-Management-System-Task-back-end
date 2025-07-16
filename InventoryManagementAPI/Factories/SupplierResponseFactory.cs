using InventoryManagementAPI.DTOs.Supplier;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public class SupplierResponseFactory : ISupplierResponseFactory
    {
        public SupplierResponseDto CreateSupplierResponse(Supplier supplier)
        {
            return new SupplierResponseDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                ContactName = supplier.ContactName,
                Phone = supplier.Phone,
                Email = supplier.Email,
                Address = supplier.Address,
                CreatedOn = supplier.CreatedOn
            };
        }

        public IEnumerable<SupplierResponseDto> CreateSupplierResponses(IEnumerable<Supplier> suppliers)
        {
            return suppliers.Select(CreateSupplierResponse);
        }
    }
}
