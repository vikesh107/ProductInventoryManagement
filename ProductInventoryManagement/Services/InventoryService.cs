using ProductInventoryManagement.DTOs;
using ProductInventoryManagement.Models;
using ProductInventoryManagement.Repositories;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductInventoryManagement.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private const int lowThreshold = 10; // Example threshold value

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<InventoryDTO>> GetInventoriesAsync()
        {
            var inventories = await _inventoryRepository.GetAllAsync();
            return inventories.Select(i => new InventoryDTO
            {
                InventoryID = i.InventoryID,
                ProductID = i.ProductID,
                Quantity = i.Quantity,
                WarehouseLocation = i.WarehouseLocation,
                LastUpdated = i.LastUpdated
            });
        }

        public async Task<Inventory> GetInventoryByIdAsync(Guid id)
        {
            return await _inventoryRepository.GetByIdAsync(id);
        }

        public async Task<InventoryDTO> GetInventoryByProductIdAsync(Guid productId)
        {
            var inventory = await _inventoryRepository.GetInventoryByProductIdAsync(productId);
            if (inventory == null) return null;

            return new InventoryDTO
            {
                InventoryID = inventory.InventoryID,
                ProductID = inventory.ProductID,
                Quantity = inventory.Quantity,
                WarehouseLocation = inventory.WarehouseLocation,
                LastUpdated = inventory.LastUpdated
            };
        }
        public async Task AddInventoryAsync(InventoryDTO inventoryDto)
        {
            var inventory = new Inventory
            {
                InventoryID = Guid.NewGuid(),
                ProductID = inventoryDto.ProductID,
                Quantity = inventoryDto.Quantity,
                WarehouseLocation = inventoryDto.WarehouseLocation,
                LastUpdated = DateTime.UtcNow
            };

            await _inventoryRepository.AddAsync(inventory);
        }


        public async Task UpdateInventoryAsync(InventoryDTO inventoryDto)
        {
            // Fetch the existing inventory entity from the repository
            var inventory = await _inventoryRepository.GetByIdAsync(inventoryDto.InventoryID);

            if (inventory != null)
            {
                inventory.Quantity = inventoryDto.Quantity;
                inventory.WarehouseLocation = inventoryDto.WarehouseLocation;
                inventory.LastUpdated = DateTime.UtcNow;

                await _inventoryRepository.UpdateAsync(inventory);
            }
        }


        public async Task DeleteInventoryAsync(Guid id)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(id);
            if (inventory != null)
            {
                await _inventoryRepository.RemoveAsync(inventory);
            }
        }


        public async Task AddQuantityAsync(Guid productId, int quantity)
        {
            await _inventoryRepository.AddQuantityAsync(productId, quantity);
        }

        public async Task ReduceQuantityAsync(Guid productId, int quantity)
        {
            await _inventoryRepository.ReduceQuantityAsync(productId, quantity);
        }

        public async Task<bool> CheckLowInventoryAsync()
        {
            var inventories = await _inventoryRepository.GetAllAsync();
            bool LowThreshold = false;
            foreach (var inventory in inventories)
            {
                if (inventory.Quantity < lowThreshold)
                {
                    NotifyRestock(inventory);
                    LowThreshold = true;
                }
            }
            return LowThreshold;
        }

        private void NotifyRestock(Inventory inventory)
        {
            // This is where you implement your notification logic
            Log.Information($"Low inventory alert for Product ID: {inventory.ProductID}. Current quantity: {inventory.Quantity}. Consider restocking!");
        }

    }
}
