using InventoryManagementAPI.DTOs.StockMovement;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public interface IStockMovementService
    {
        Task<StockMovement?> CreateStockMovementAsync(CreateStockMovementRequestDto request, int userId);
        Task<IEnumerable<StockMovement>> GetLastStockMovementsAsync(int productId, int count = 10);
    }
}
