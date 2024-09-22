using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductInventoryManagement.Data;
using ProductInventoryManagement.DTO;
using ProductInventoryManagement.Models;
using System.Linq.Expressions;

namespace ProductInventoryManagement.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductsDTO>> GetAllProductsAsync()
        {

            return await _context.Products.AsNoTracking().Select(x => new ProductsDTO
            {
                ProductID = x.ProductID,
                Price = x.Price,
                SKU = x.SKU,
                Name = x.Name,
                Description = x.Description,
            }
            ).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(Guid? categoryId, string categoryName)
        {
            var query = _context.Products
                        .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category)
                        .AsQueryable();

            // Check if categoryId is provided
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.ProductCategories.Any(pc => pc.CategoryID == categoryId.Value));
            }

            // Check if categoryName is provided
            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(p => p.ProductCategories.Any(pc => pc.Category.Name.Contains(categoryName)));
            }

            return await query.ToListAsync();
        }

        public async Task<List<Product>> GetProductsByIdsAsync(List<Guid> productIds)
        {
            return await _context.Products.Where(p => productIds.Contains(p.ProductID)).ToListAsync();
        }

    }
}
