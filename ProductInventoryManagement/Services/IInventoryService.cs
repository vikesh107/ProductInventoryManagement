using System.Collections.Generic;
using System.Threading.Tasks;
using ProductInventoryManagement.DTOs;
using ProductInventoryManagement.Models;

namespace ProductInventoryManagement.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryDTO>> GetInventoriesAsync();
        Task<Inventory> GetInventoryByIdAsync(Guid id);
        Task AddInventoryAsync(InventoryDTO inventory);
        Task UpdateInventoryAsync(InventoryDTO inventory);
        Task DeleteInventoryAsync(Guid id);
        Task AddQuantityAsync(Guid productId, int quantity);
        Task ReduceQuantityAsync(Guid productId, int quantity);
        Task<InventoryDTO> GetInventoryByProductIdAsync(Guid productId);
        Task<bool> CheckLowInventoryAsync();
    }
}
