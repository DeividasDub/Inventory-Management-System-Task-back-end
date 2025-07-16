using InventoryManagementAPI.DTOs.StockMovement;

namespace InventoryManagementAPI.Services
{
    public interface IStockMovementService
    {
        Task<StockMovementResponseDto?> CreateStockMovementAsync(CreateStockMovementRequestDto request, int userId);
        Task<IEnumerable<StockMovementResponseDto>> GetLastStockMovementsAsync(int productId, int count = 10);
    }
}
