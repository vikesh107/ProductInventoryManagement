using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductInventoryManagement.Data;
using ProductInventoryManagement.Models;
using System;
using System.Threading.Tasks;

namespace ProductInventoryManagement.Repositories
{
    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
     
        public async Task<Inventory> GetInventoryByProductIdAsync(Guid productId)
        {
            return await _context.Inventories.FirstOrDefaultAsync(i => i.ProductID == productId);
        }

        public async Task AddQuantityAsync(Guid productId, int quantity)
        {
            var inventory = await GetInventoryByProductIdAsync(productId);
            if (inventory != null)
            {
                inventory.Quantity += quantity;
                inventory.LastUpdated = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ReduceQuantityAsync(Guid productId, int quantity)
        {
            var inventory = await GetInventoryByProductIdAsync(productId);
            if (inventory != null && inventory.Quantity >= quantity)
            {
                inventory.Quantity -= quantity;
                inventory.LastUpdated = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
