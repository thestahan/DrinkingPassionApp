using DrinkingPassion.WebApp.Features.Cocktails.Dtos;
using DrinkingPassion.WebApp.Services.Interfaces;
using DrinkingPassion.WebApp.Shared;
using System.Net.Http.Json;

namespace DrinkingPassion.WebApp.Services;

public class CocktailsService : ICocktailsService
{
    private readonly HttpClient _httpClient;

    public CocktailsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CocktailDetails?> GetCocktailDetails(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/cocktails/{id}");

        return await _httpClient.GetFromJsonAsync<CocktailDetails>(request.RequestUri!.ToString());
    }

    public async Task<Pagination<CocktailDto>?> GetPublicCocktails(int pageIndex)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/cocktails/public?pageIndex={pageIndex}");

        return await _httpClient.GetFromJsonAsync<Pagination<CocktailDto>>(request.RequestUri!.ToString());
    }
}
