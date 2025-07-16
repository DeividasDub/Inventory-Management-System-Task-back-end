using InventoryManagementAPI.DTOs.StockMovement;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public interface IStockMovementResponseFactory
    {
        StockMovementResponseDto CreateStockMovementResponse(StockMovement stockMovement);
        IEnumerable<StockMovementResponseDto> CreateStockMovementResponses(IEnumerable<StockMovement> stockMovements);
    }
}
