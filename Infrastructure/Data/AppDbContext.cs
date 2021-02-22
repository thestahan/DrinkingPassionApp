using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Cocktail> Cocktails { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductUnit> ProductUnits { get; set; }
    }
}
