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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDTO model)
        {
            await _productService.AddProductAsync(model);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductDTO model)
        {
            await _productService.UpdateProductAsync(id, model);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("products-by-category")]
        public async Task<ActionResult<List<ProductsDTO>>> GetProductsByCategory(Guid? categoryId, string categoryName)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId, categoryName);
            return Ok(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("adjust-prices")]
        public async Task<IActionResult> AdjustPrices([FromBody] PriceAdjustmentDTO adjustment)
        {
            try
            {
                await _productService.AdjustPricesAsync(adjustment);
                return Ok("Prices adjusted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adjusting prices: {ex.Message}");
            }
        }

    }
}
