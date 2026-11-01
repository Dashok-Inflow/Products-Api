using AutoMapper;
using ProductsApi.Models;
using ProductsApi.ModelsDto.CategoriesDto;

namespace ProductsApi.Mappings
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
        }
    }
}
