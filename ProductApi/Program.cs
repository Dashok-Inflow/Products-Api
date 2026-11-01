using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductApi.Middlewares;
using ProductsApi.Data;
using ProductsApi.Services.Auth;
using ProductsApi.Services.CategoryService;
using ProductsApi.Services.ProductS;
using ProductsApi.Services.ProductService.Product;
using Serilog;
using System.Security.Cryptography.Xml;
using System.Text;

namespace ProductApi
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Запуск приложения...");

                var builder = WebApplication.CreateBuilder(args);
                builder.Host.UseSerilog();

                builder.Services.AddScoped<IAuthService, AuthService>();
                builder.Services.AddScoped<IProductService, ProductService>();
                builder.Services.AddScoped<ICategoryService, CategoryService>();
                builder.Services.AddLogging();
                builder.Services.AddAutoMapper(typeof(Program));
                builder.Services.AddControllers();

                builder.Services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Введите ваш JWT токен"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                    });
                });

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlite("Data Source=prod.db"));

                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["Jwt:Issuer"],
                            ValidAudience = builder.Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey
                                (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                        };
                    });


                var app = builder.Build();
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    db.Database.EnsureCreated(); 
                }

                app.UseSerilogRequestLogging();
                app.UseMiddleware<ExceptionHandlingMiddleware>();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch(Exception ex)
            {
                Log.Fatal("Приложение упало при запуске");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
    public partial class Program { }
}
