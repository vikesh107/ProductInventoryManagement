using ProductInventoryManagement.DTO;
using ProductInventoryManagement.Models;

namespace ProductInventoryManagement.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<ProductsDTO>> GetAllProductsAsync();
        Task<List<Product>> GetProductsByCategoryAsync(Guid? categoryId, string name);
        Task<List<Product>> GetProductsByIdsAsync(List<Guid> productIds);
    }
}
