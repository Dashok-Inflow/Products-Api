using System.ComponentModel.DataAnnotations;

namespace ProductsApi.ModelsDto.ProductsDto
{
    public class CreateProductDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Слишком короткое название")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 100, ErrorMessage = "Стоимость должна быть больше нуля")]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
