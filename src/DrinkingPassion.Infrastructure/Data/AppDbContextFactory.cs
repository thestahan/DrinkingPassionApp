using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DrinkingPassion.Api.Infrastructure.Data
{
    /// <summary>
    /// Factory class for creating AppDbContext instances during design-time operations like
    /// migrations. This class is used by Entity Framework Core tools when running commands like
    /// 'dotnet ef migrations add' or 'dotnet ef database update' without starting the application.
    /// It ensures migrations are created in the correct directory with the proper configuration.
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <summary>
        /// Creates a new instance of AppDbContext for design-time services.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service consumer.</param>
        /// <returns>A configured instance of AppDbContext.</returns>
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("LocalPostgresConnection");

            optionsBuilder.UseNpgsql(connectionString,
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "public")
                      .MigrationsAssembly("Infrastructure"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
