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

    public async Task<Pagination<CocktailToReturnDto>?> GetPublicCocktails(
        int pageIndex,
        int pageSize,
        string? cocktailName = null,
        int? ingredientsExactCount = null,
        List<int>? ingredientsList = null,
        string? sort = null)
    {
        var queryParams = new List<string>
        {
            $"pageIndex={pageIndex}",
            $"pageSize={pageSize}"
        };

        if (!string.IsNullOrWhiteSpace(cocktailName))
        {
            queryParams.Add($"cocktailName={Uri.EscapeDataString(cocktailName)}");
        }

        if (ingredientsExactCount.HasValue)
        {
            queryParams.Add($"ingredientsExactCount={ingredientsExactCount.Value}");
        }

        if (ingredientsList != null && ingredientsList.Count != 0)
        {
            foreach (var ingredientId in ingredientsList)
            {
                queryParams.Add($"ingredientsList={ingredientId}");
            }
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            queryParams.Add($"sort={Uri.EscapeDataString(sort)}");
        }

        string queryString = string.Join("&", queryParams);

        string requestUri = $"api/cocktails/public?{queryString}";

        return await _httpClient.GetFromJsonAsync<Pagination<CocktailToReturnDto>>(requestUri);
    }
}
