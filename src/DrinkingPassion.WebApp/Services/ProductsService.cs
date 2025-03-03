using DrinkingPassion.WebApp.Features.Products.Dtos;
using DrinkingPassion.WebApp.Services.Interfaces;
using System.Net.Http.Json;

namespace DrinkingPassion.WebApp.Services;

public class ProductsService : IProductsService
{
    private readonly HttpClient _httpClient;

    public ProductsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateProduct(ProductToAddDto product)
    {
        await _httpClient.PostAsJsonAsync("api/products", product);
    }

    public async Task DeleteProductAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/products/{id}");
    }

    public async Task<ProductDto> GetProduct(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/products/{id}");

        return await _httpClient.GetFromJsonAsync<ProductDto>(request.RequestUri!.ToString())!;
    }

    public async Task<ICollection<ProductDto>> GetProducts()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/products/public");

        return await _httpClient.GetFromJsonAsync<ICollection<ProductDto>>(request.RequestUri!.ToString())!;
    }

    public async Task UpdateProduct(int id, ProductToUpdateDto product)
    {
        await _httpClient.PutAsJsonAsync($"api/products/{id}", product);
    }
}
