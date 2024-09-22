using Microsoft.EntityFrameworkCore;
using ProductInventoryManagement.DTO;
using ProductInventoryManagement.Models;
using ProductInventoryManagement.Repositories;

namespace ProductInventoryManagement.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(Guid id)
        {
            var product = await _categoryRepository.GetByIdAsync(id);
            if (product == null)
                return null;

            var catagory = new CategoryDTO
            {
                CategoryID = product.CategoryID,
                Name = product.Name
            };
            return catagory;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDTO
            {
                CategoryID = c.CategoryID,
                Name = c.Name
            });
        }

        public async Task<bool> AddCategoryAsync(AddCategoryModel category)
        {
            var catagory = new Category
            {
                CategoryID = Guid.NewGuid(),
                Name = category.Name,
            };
            await _categoryRepository.AddAsync(catagory);
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(Guid id, UpdateCategoryModel model)
        {
            var catagory = await _categoryRepository.GetByIdAsync(id);
            if (catagory == null)
                return false;

            catagory.Name = model.Name;
            await _categoryRepository.UpdateAsync(catagory);

            return true;
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.RemoveAsync(category);
            }
        }
    }
}
