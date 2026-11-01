using ProductsApi.ModelsDto.AuthDto;

namespace ProductsApi.Services.Auth
{
    public interface IAuthService
    {
        Task RegisterUserAsync(RegisterUserDto userDto);
        Task<TokenResponseDto> LoginUserAsync(LoginUserDto loginUserDto);
    }
}
