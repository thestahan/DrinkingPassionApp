using DrinkingPassion.Shared.Models;
using DrinkingPassion.Shared.Models.Cocktails;
using DrinkingPassion.WebApp.Services.Interfaces;
using System.Net.Http.Json;

namespace DrinkingPassion.WebApp.Services;

public class CocktailsService : ICocktailsService
{
    private readonly HttpClient _httpClient;

    public CocktailsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CocktailDetailsToReturnDto?> GetCocktailDetails(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/cocktails/{id}");

        return await _httpClient.GetFromJsonAsync<CocktailDetailsToReturnDto>(request.RequestUri!.ToString());
    }

    public async Task<Pagination<CocktailToReturnDto>?> GetPublicCocktails(int pageIndex)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/cocktails/public?pageIndex={pageIndex}");

        return await _httpClient.GetFromJsonAsync<Pagination<CocktailToReturnDto>>(request.RequestUri!.ToString());
    }
}
