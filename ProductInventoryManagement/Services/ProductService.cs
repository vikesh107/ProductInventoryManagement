using Microsoft.EntityFrameworkCore;
using ProductInventoryManagement.DTO;
using ProductInventoryManagement.Models;
using ProductInventoryManagement.Repositories;

namespace ProductInventoryManagement.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<ProductsDTO>> GetAllProductsAsync()
        {
            var productCategoriesQuery = from product in _productRepository.GetAll().AsNoTracking()
                                         join productCategory in _productCategoryRepository.GetAll().AsNoTracking() on product.ProductID equals productCategory.ProductID
                                         join category in _categoryRepository.GetAll().AsNoTracking() on productCategory.CategoryID equals category.CategoryID
                                         select new
                                         {
                                             product.ProductID,
                                             product.Name,
                                             product.Price,
                                             product.Description,
                                             product.SKU,
                                             CategoryName = category.Name
                                         };

            // Execute query and materialize the result in memory
            var productCategoryData = await productCategoriesQuery.ToListAsync();

            // Grouping by product and aggregating category names
            var productsGrouped = productCategoryData
                .GroupBy(p => new { p.ProductID, p.Name, p.Price, p.Description, p.SKU })
                .Select(group => new ProductsDTO
                {
                    ProductID = group.Key.ProductID,
                    Name = group.Key.Name,
                    Price = group.Key.Price,
                    Description = group.Key.Description,
                    SKU = group.Key.SKU,
                    CategoryName = group.Select(p => p.CategoryName).Distinct().ToList() // Handle distinct category names
                }).ToList();

            return productsGrouped;
        }

        public async Task<ProductsDTO> GetProductByIdAsync(Guid id)
        {

            var product = await _productRepository
                .GetAll()
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
            {
                return null;
            }
            var productDTO = new ProductsDTO
            {
                SKU = product.SKU,
                Price = product.Price,
                Description = product.Description,
                Name = product.Name,
                ProductID = product.ProductID,
                CategoryName = product.ProductCategories
                                    .Select(pc => pc.Category.Name)
                                    .ToList()
            };

            return productDTO;
        }


        public async Task AddProductAsync(AddProductDTO model)
        {
            var product = new Product
            {
                ProductID = Guid.NewGuid(),
                Name = model.Name,
                SKU = model.SKU,
                Price = model.Price,
                Description = model.Description,
                CreatedDate = DateTime.UtcNow
            };
            await _productRepository.AddAsync(product);
            foreach (Guid catagaroyID in model.catgeroyID)
            {
                var productCatagoey = new ProductCategory
                {
                    ProductCategoryId = Guid.NewGuid(),
                    CategoryID = catagaroyID,
                    ProductID = product.ProductID
                };
                await _productCategoryRepository.AddAsync(productCatagoey);
            }
        }

        public async Task<bool> UpdateProductAsync(Guid id, UpdateProductDTO model)
        {
            // Fetch the existing product with related categories
            var existingProduct = await _productRepository
                .GetAll() // Assuming this returns IQueryable<Product>
                .Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (existingProduct == null)
                return false;

            bool isUpdated = false;

            // Update product details if they are changed
            if (existingProduct.Price != model.Price)
            {
                existingProduct.Price = model.Price;
                isUpdated = true;
            }
            if (existingProduct.Name != model.Name)
            {
                existingProduct.Name = model.Name;
                isUpdated = true;
            }
            if (existingProduct.SKU != model.SKU)
            {
                existingProduct.SKU = model.SKU;
                isUpdated = true;
            }
            if (existingProduct.Description != model.Description)
            {
                existingProduct.Description = model.Description;
                isUpdated = true;
            }

            // Handle category updates
            var existingCategoryIds = existingProduct.ProductCategories.Select(pc => pc.CategoryID).ToList();
            var newCategoryIds = model.CategoryIDs;

            // Categories to remove
            var categoriesToRemove = existingProduct.ProductCategories
                .Where(pc => !newCategoryIds.Contains(pc.CategoryID))
                .ToList();

            // Categories to add
            var categoriesToAdd = newCategoryIds
                .Where(cid => !existingCategoryIds.Contains(cid))
                .Select(cid => new ProductCategory
                {
                    CategoryID = cid,
                    ProductID = existingProduct.ProductID
                })
                .ToList();

            // Remove categories that are no longer associated with the product
            if (categoriesToRemove.Any())
            {
                await _productCategoryRepository.RemoveRangeAsync(categoriesToRemove); // Assuming RemoveRangeAsync exists
                isUpdated = true;
            }

            // Add new categories that are now associated with the product
            if (categoriesToAdd.Any())
            {
                await _productCategoryRepository.AddRangeAsync(categoriesToAdd); // Assuming AddRangeAsync exists
                isUpdated = true;
            }

            // Update the product if any changes were made
            if (isUpdated)
            {
                await _productRepository.UpdateAsync(existingProduct);
            }

            return isUpdated;
        }


        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _productRepository
                .GetAll()
                .Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product != null)
            {
                var categoriesToRemove = product.ProductCategories.ToList();
                if (categoriesToRemove.Any())
                {
                    await _productCategoryRepository.RemoveRangeAsync(categoriesToRemove);
                }
                await _productRepository.RemoveAsync(product);
            }
        }

        public async Task<List<ProductsDTO>> GetProductsByCategoryAsync(Guid? categoryId, string categoryName)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId, categoryName);
            return products.Select(p => new ProductsDTO
            {
                ProductID = p.ProductID,
                SKU = p.SKU,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryName = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
            }).ToList();
        }


        public async Task AdjustPricesAsync(PriceAdjustmentDTO adjustment)
        {
            // Check for negative values
            if (adjustment.Percentage < 0)
                throw new ArgumentException("Percentage cannot be negative.");

            if (adjustment.FixedAmount < 0)
                throw new ArgumentException("Fixed amount cannot be negative.");

            if (adjustment.Percentage == 0 && adjustment.FixedAmount == 0)
                throw new ArgumentException("No adjustment to apply.");

            var products = await _productRepository.GetProductsByIdsAsync(adjustment.ProductIDs);

            // Check if any products found
            if (!products.Any())
                throw new KeyNotFoundException("No products found for the provided IDs.");

            foreach (var product in products)
            {
                // Check for price limits
                if (product.Price == 0 && adjustment.FixedAmount > 0)
                    throw new InvalidOperationException("Price cannot be decreased further; it is already at minimum.");

                if (adjustment.Percentage > 100)
                    throw new ArgumentException("Percentage cannot exceed 100.");

                if (adjustment.FixedAmount > product.Price)
                    throw new ArgumentException($"Fixed amount cannot exceed the current price of {product.Price}.");

                // Apply adjustments
                if (adjustment.Percentage.HasValue)
                    product.Price -= product.Price * (adjustment.Percentage.Value / 100);

                if (adjustment.FixedAmount.HasValue)
                    product.Price -= adjustment.FixedAmount.Value;

                // Prevent negative prices
                if (product.Price < 0)
                    product.Price = 0;

                product.LastUpdatedDate = DateTime.UtcNow;
            }

            await _productRepository.UpdateRangeAsync(products);
        }
    }
}
