using DrinkingPassion.Api.Extensions;
using DrinkingPassion.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DrinkingPassion.Api;

public class Startup
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public Startup(IWebHostEnvironment env, IConfiguration config)
    {
        _env = env;
        _config = config;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<Middleware.ExceptionMiddleware>();

        if (env.IsDevelopment())
        {
            app.UseSwaggerDocumentation();
        }

        app.UseStatusCodePagesWithReExecute("/errors/{0}");
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseStaticFiles();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
            endpoints.MapControllers());
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        var connection = string.Empty;
        if (_env.IsDevelopment())
        {
            connection = _config.GetConnectionString("LocalPostgresConnection");
        }
        else
        {
            connection = Environment.GetEnvironmentVariable("POSTGRES_CONNECTIONSTRING");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connection, x =>
                x.MigrationsHistoryTable("__EFMigrationsHistory", "public")
                 .MigrationsAssembly("Infrastructure")));

        services.AddApplicationServices(_config);
        services.AddIdentityServices(_config);
        services.AddSwaggerDocumentation();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}
