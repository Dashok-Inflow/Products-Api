using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using ProductsApi.Models;

namespace ProductsApi.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions context):base(context)
        {

        }
    }
}
