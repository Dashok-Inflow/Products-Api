using ProductsApi.ModelsDto.CategoriesDto;

namespace ProductsApi.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync(string? category);

        Task<CategoryDto> GetCategoryByIdAsync(int id);

        Task<CategoryDto> CreateNewCategoryAsync(CreateCategoryDto categoryDto);

        Task<CategoryDto> UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto);

        Task DeleteCategoryAsync(int id);
    }
}
