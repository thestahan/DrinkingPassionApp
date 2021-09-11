using System;
using System.Threading.Tasks;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Data.Migrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    await context.Database.MigrateAsync();
                    await AppDataDbContextSeed.SeedDataAsync(context);

                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await AppRolesDbContextSeed.SeedRolesAsync(roleManager);

                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    await AppIdentityDbContextSeed.SeedUserAsync(userManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occured during migration");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        return;
                    }

                    var clientSecretCred = new ClientSecretCredential(Environment.GetEnvironmentVariable("KEY_VAULT_TENANT_ID"),
                                                                      Environment.GetEnvironmentVariable("KEY_VAULT_CLIENT_ID"),
                                                                      Environment.GetEnvironmentVariable("KEY_VAULT_CLIENT_SECRET"));

                    var client = new SecretClient(new Uri($"https://{Environment.GetEnvironmentVariable("KEY_VAULT_NAME")}.vault.azure.net/"), clientSecretCred);

                    config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
