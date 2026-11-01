using System.ComponentModel.DataAnnotations;

namespace ProductsApi.ModelsDto.ProductsDto
{
    public class UpdateProductDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Короткое название")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100, ErrorMessage = "Цена не может быть равно нулю")]
        public  decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
