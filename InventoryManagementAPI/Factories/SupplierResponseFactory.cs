using InventoryManagementAPI.DTOs.Supplier;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public class SupplierModelFactory : ISupplierModelFactory
    {
        public SupplierResponseDto PrepareSupplierResponseModel(Supplier supplier)
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

        public IEnumerable<SupplierResponseDto> PrepareSupplierListResponseModel(IEnumerable<Supplier> suppliers)
        {
            return suppliers.Select(PrepareSupplierResponseModel);
        }
    }
}
