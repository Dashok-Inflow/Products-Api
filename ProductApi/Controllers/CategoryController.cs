using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.ModelsDto.CategoriesDto;
using ProductsApi.Services.CategoryService;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAll([FromQuery] string? category)
        {
            var result = await categoryService.GetAllAsync(category);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var result = await categoryService.GetCategoryByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateNewCategory([FromBody] CreateCategoryDto categoryDto)
        {
            var result = await categoryService.CreateNewCategoryAsync(categoryDto);
            return CreatedAtAction(nameof(GetCategoryById), new { Id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] UpdateCategoryDto categoryDto)
        {
            var result = await categoryService.UpdateCategoryAsync(id, categoryDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
