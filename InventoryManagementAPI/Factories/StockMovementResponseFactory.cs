using InventoryManagementAPI.DTOs.StockMovement;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Factories
{
    public class StockMovementResponseFactory : IStockMovementResponseFactory
    {
        public StockMovementResponseDto CreateStockMovementResponse(StockMovement stockMovement)
        {
            return new StockMovementResponseDto
            {
                Id = stockMovement.Id,
                ProductId = stockMovement.ProductId,
                ProductName = stockMovement.Product?.Name ?? string.Empty,
                ProductSKU = stockMovement.Product?.SKU ?? string.Empty,
                Type = stockMovement.Type,
                Quantity = stockMovement.Quantity,
                Reason = stockMovement.Reason,
                Date = stockMovement.Date,
                CreatedByUserId = stockMovement.CreatedByUserId,
                CreatedByUserName = stockMovement.CreatedByUser != null ? 
                    $"{stockMovement.CreatedByUser.FirstName} {stockMovement.CreatedByUser.LastName}" : string.Empty
            };
        }

        public IEnumerable<StockMovementResponseDto> CreateStockMovementResponses(IEnumerable<StockMovement> stockMovements)
        {
            return stockMovements.Select(CreateStockMovementResponse);
        }
    }
}
