using Blazored.LocalStorage;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace DrinkingPassion.WebApp.Startup;

public static class ServicesBuilder
{
    private static TSettings BindConfiguration<TSettings>(WebAssemblyHostBuilder builder) where TSettings : new()
    {
        var settings = new TSettings();
        builder.Configuration.Bind(settings);
        return settings;
    }

    private static string? GetBaseApiAddress(WebAssemblyHostBuilder builder)
    {
        return builder.HostEnvironment.IsDevelopment()
            ? builder.Configuration["BaseApiAddress"]
            : Environment.GetEnvironmentVariable("BaseApiAddress");
    }

    public static void ConfigureServices(WebAssemblyHostBuilder builder)
    {
        var settings = BindConfiguration<Shared.AppSettings>(builder);
        builder.Services.AddSingleton(settings);

        string baseApiAddress = GetBaseApiAddress(builder)
            ?? throw new ArgumentException("BaseApiAddress is not set");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseApiAddress) });
        builder.Services.AddFluxor(o =>
        {
            o.ScanAssemblies(typeof(Program).Assembly);
#if DEBUG
            o.UseReduxDevTools();
#endif
        });
        builder.Services.AddMudServices();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddScoped<Services.Interfaces.ICocktailsService, Services.CocktailsService>();
        builder.Services.AddScoped<Services.Interfaces.IUsersService, Services.UsersService>();
        builder.Services.AddScoped<Services.Interfaces.IProductsService, Services.ProductsService>();
        builder.Services.AddScoped<Features.Auth.DrinkingPassionAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(
            sp => sp.GetRequiredService<Features.Auth.DrinkingPassionAuthenticationStateProvider>());
        builder.Services.AddAuthorizationCore();
    }
}
