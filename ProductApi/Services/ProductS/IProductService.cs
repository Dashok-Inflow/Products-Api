using ProductsApi.ModelsDto.ProductsDto;

namespace ProductsApi.Services.ProductService.Product
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync(string? category);

        Task<ProductDto> GetProductByIdAsync(int id);

        Task<ProductDto> CreateNewProductAsync(CreateProductDto productDto);

        Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto productDto);

        Task DeleteProductAsync(int id);
    }
}
