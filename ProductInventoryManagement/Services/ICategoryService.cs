using ProductInventoryManagement.DTO;
using ProductInventoryManagement.Models;

namespace ProductInventoryManagement.Services
{
    public interface ICategoryService
    {
        Task<CategoryDTO> GetCategoryByIdAsync(Guid id);
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<bool> AddCategoryAsync(AddCategoryModel category);
        Task<bool> UpdateCategoryAsync(Guid id, UpdateCategoryModel category);
        Task DeleteCategoryAsync(Guid id);
    }
}
