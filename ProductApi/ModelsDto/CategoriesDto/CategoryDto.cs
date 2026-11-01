using ProductsApi.ModelsDto.ProductsDto;

namespace ProductsApi.ModelsDto.CategoriesDto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<ProductDto> Products { get; set; } = new();
    }
}
