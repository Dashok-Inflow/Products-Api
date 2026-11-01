using System.ComponentModel.DataAnnotations;

namespace ProductsApi.ModelsDto.CategoriesDto
{
    public class UpdateCategoryDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Слишком короткое/длинное название категории")]
        public string Name { get; set; } = string.Empty;
    }
}
