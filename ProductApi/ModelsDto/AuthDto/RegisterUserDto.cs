using System.ComponentModel.DataAnnotations;

namespace ProductsApi.ModelsDto.AuthDto
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен состоять из 6 до 100 символов")]
        public string Password { get; set; } = string.Empty;
    }
}
