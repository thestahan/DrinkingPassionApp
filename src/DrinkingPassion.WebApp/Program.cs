using DrinkingPassion.WebApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

DrinkingPassion.WebApp.Startup.ServicesBuilder.ConfigureServices(builder);

await builder.Build().RunAsync();
