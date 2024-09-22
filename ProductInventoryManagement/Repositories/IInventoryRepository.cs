using System.Collections.Generic;
using System.Threading.Tasks;
using ProductInventoryManagement.Models;

namespace ProductInventoryManagement.Repositories
{
    public interface IInventoryRepository : IRepository<Inventory>
    { 

        Task<Inventory> GetInventoryByProductIdAsync(Guid productId);
        Task AddQuantityAsync(Guid productId, int quantity);
        Task ReduceQuantityAsync(Guid productId, int quantity);
    }
}
