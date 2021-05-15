using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Cocktail> Cocktails { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductUnit> ProductUnits { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Cocktail>()
                .HasOne(x => x.BaseProduct)
                .WithMany()
                .HasForeignKey(x => x.BaseProductId);
        }
    }
}
