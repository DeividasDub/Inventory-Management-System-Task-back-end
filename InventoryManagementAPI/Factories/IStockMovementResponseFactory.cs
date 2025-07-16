using InventoryManagementAPI.DTOs.StockMovement;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public interface IStockMovementModelFactory
    {
        StockMovementResponseDto PrepareStockMovementResponseModel(StockMovement stockMovement);
        IEnumerable<StockMovementResponseDto> PrepareStockMovementListResponseModel(IEnumerable<StockMovement> stockMovements);
    }
}
