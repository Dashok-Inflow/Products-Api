using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductApi.Exceptions;
using ProductsApi.Data;
using ProductsApi.Models;
using ProductsApi.ModelsDto.CategoriesDto;

namespace ProductsApi.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CategoryDto> CreateNewCategoryAsync(CreateCategoryDto categoryDto)
        {
            var category = mapper.Map<Category>(categoryDto);

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return mapper.Map<CategoryDto>(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                throw new NotFoundException("Такой категории товаров не существует");
            }

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }

        public async Task<List<CategoryDto>> GetAllAsync(string? category)
        {
            var query = context.Categories.Include(c => c.Products).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(c => c.Name.Contains(category));
            }
            var result = await query.ToListAsync();

            return mapper.Map<List<CategoryDto>>(result);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                throw new NotFoundException("Нет такой категории продукта");
            }

            return mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(int id, UpdateCategoryDto categoryDto)
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                throw new NotFoundException("Такой категории товаров не существует");
            }

            mapper.Map(categoryDto, category);
            await context.SaveChangesAsync();

            var result = mapper.Map<CategoryDto>(category);
            return result;
        }
    }
}
