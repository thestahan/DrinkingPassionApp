using DrinkingPassion.Shared.Models.Products;
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

    public async Task<ProductToReturnDto?> GetProduct(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/products/{id}");

        return await _httpClient.GetFromJsonAsync<ProductToReturnDto>(request.RequestUri!.ToString());
    }

    public async Task<IReadOnlyList<ProductToReturnDto>> GetProducts()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/products/public");

        var result = await _httpClient.GetFromJsonAsync<IReadOnlyList<ProductToReturnDto>>(request.RequestUri!.ToString());

        return result ?? new List<ProductToReturnDto>().AsReadOnly();
    }

    public async Task UpdateProduct(int id, ProductToUpdateDto product)
    {
        await _httpClient.PutAsJsonAsync($"api/products/{id}", product);
    }
}
