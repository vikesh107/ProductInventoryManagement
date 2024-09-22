using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductInventoryManagement.DTO;
using ProductInventoryManagement.Models;
using ProductInventoryManagement.Services;

namespace ProductInventoryManagement.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")] // Specify the version
    public class AppCategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public AppCategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddCategory([FromBody] AddCategoryModel model)
        {
            var category  = await _categoryService.AddCategoryAsync(model);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateCategory(Guid id, [FromBody] UpdateCategoryModel model)
        {
            var data = await _categoryService.UpdateCategoryAsync(id, model);
            return Ok(data);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<ActionResult> GetAllategories()
        {
            var data = await _categoryService.GetAllCategoriesAsync();
            return Ok(data);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
        }
    }
}
