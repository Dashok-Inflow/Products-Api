using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.ModelsDto.ProductsDto;
using ProductsApi.Services.ProductService.Product;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAll([FromQuery] string? category)
        {
            var result = await productService.GetAllAsync(category);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var result = await productService.GetProductByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateNewProduct([FromBody] CreateProductDto createProductDto)
        {
            var result = await productService.CreateNewProductAsync(createProductDto);
            return CreatedAtAction(nameof(GetProductById), new { Id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            var result = await productService.UpdateProductAsync(id, updateProductDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
