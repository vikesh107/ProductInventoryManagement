using ProductInventoryManagement.DTO;
using ProductInventoryManagement.Models;

namespace ProductInventoryManagement.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductsDTO>> GetAllProductsAsync();
        Task<ProductsDTO> GetProductByIdAsync(Guid id);
        Task AddProductAsync(AddProductDTO model);
        Task<bool> UpdateProductAsync(Guid id, UpdateProductDTO product);
        Task DeleteProductAsync(Guid id);
        Task<List<ProductsDTO>> GetProductsByCategoryAsync(Guid? categoryId, string name);
        Task AdjustPricesAsync(PriceAdjustmentDTO adjustment);
    }
}
