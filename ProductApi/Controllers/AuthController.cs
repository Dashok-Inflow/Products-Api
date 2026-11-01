using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.ModelsDto.AuthDto;
using ProductsApi.Services.Auth;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            await authService.RegisterUserAsync(registerUserDto);
            return Ok(new { message = "Пользователь успешно зарегистрирован"});
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginUserDto loginUserDto)
        {
            var result = await authService.LoginUserAsync(loginUserDto);
            return Ok(result);
        }
    }
}
