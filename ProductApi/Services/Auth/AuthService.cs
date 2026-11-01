using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductsApi.Data;
using ProductsApi.Models;
using ProductsApi.ModelsDto.AuthDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductsApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<TokenResponseDto> LoginUserAsync(LoginUserDto loginUserDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == loginUserDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUserDto.Password, user.PasswordHash))
            {
                throw new ArgumentException("Неверный email или пароль");
            }

            var token = GenerateJwtToken(user);

            return new TokenResponseDto { Token = token };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task RegisterUserAsync(RegisterUserDto userDto)
        {
            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if(existingUser != null)
            {
                throw new ArgumentException("ПОльзователь с таким email уже существует");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var user = new User
            {
                Email = userDto.Email,
                PasswordHash = passwordHash,
                Role = "User"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
    }
}
