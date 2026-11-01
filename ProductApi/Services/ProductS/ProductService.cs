using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductApi.Exceptions;
using ProductApi.Models;
using ProductsApi.Data;
using ProductsApi.ModelsDto.ProductsDto;
using ProductsApi.Services.ProductService.Product;

namespace ProductsApi.Services.ProductS
{
    public class ProductService : IProductService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;

        public ProductService(IMapper mapper, ApplicationDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<ProductDto> CreateNewProductAsync(CreateProductDto productDto)
        {
            var product = mapper.Map<Product>(productDto);

            context.Products.Add(product);
            await context.SaveChangesAsync();

            return mapper.Map<ProductDto>(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Такого продукта не существует");
            }

            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetAllAsync(string? category)
        {
            var query = context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Name.Contains(category));
            }

            var result = await query.ToListAsync();
            return mapper.Map<List<ProductDto>>(result);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Такого продукта не существует");
            }

            return mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto productDto)
        {
            var productFromDb = await context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (productFromDb == null)
            {
                throw new NotFoundException("Таког опродукта не существует");
            }

            mapper.Map(productDto, productFromDb);
            await context.SaveChangesAsync();

            return mapper.Map<ProductDto>(productFromDb);
        }
    }
}
