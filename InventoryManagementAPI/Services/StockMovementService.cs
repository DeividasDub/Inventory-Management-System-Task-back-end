using Microsoft.EntityFrameworkCore;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.DTOs.StockMovement;
using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services
{
    public class StockMovementService : IStockMovementService
    {
        private readonly ApplicationDbContext _context;

        public StockMovementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StockMovement?> CreateStockMovementAsync(CreateStockMovementRequestDto request, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                var product = await _context.Products.FindAsync(request.ProductId);
                if (product == null)
                {
                    return null;
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return null;
                }

                if (request.Type == StockMovementType.OUT && product.QuantityInStock < request.Quantity)
                {
                    return null;
                }

                var stockMovement = new StockMovement
                {
                    ProductId = request.ProductId,
                    Type = request.Type,
                    Quantity = request.Quantity,
                    Reason = request.Reason,
                    Date = DateTime.UtcNow,
                    CreatedByUserId = userId
                };

                _context.StockMovements.Add(stockMovement);

                if (request.Type == StockMovementType.IN)
                {
                    product.QuantityInStock += request.Quantity;
                }
                else
                {
                    product.QuantityInStock -= request.Quantity;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var productWithSupplier = await _context.Products
                    .Include(p => p.Supplier)
                    .FirstAsync(p => p.Id == request.ProductId);

                stockMovement.Product = productWithSupplier;
                stockMovement.CreatedByUser = user;

                return stockMovement;
            }
            catch
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

        public async Task<IEnumerable<StockMovement>> GetLastStockMovementsAsync(int productId, int count = 10)
        {
            var stockMovements = await _context.StockMovements
                .Include(sm => sm.Product)
                .Include(sm => sm.CreatedByUser)
                .Where(sm => sm.ProductId == productId)
                .OrderByDescending(sm => sm.Date)
                .Take(count)
                .ToListAsync();

            return stockMovements;
        }
    }
}
