using DrinkingPassion.Shared.Models;
using DrinkingPassion.Shared.Models.Cocktails;

namespace DrinkingPassion.WebApp.Services.Interfaces;

public interface ICocktailsService
{
    public Task<CocktailDetailsToReturnDto?> GetCocktailDetails(int id);

    Task<Pagination<CocktailToReturnDto>> GetPublicCocktails(
        int pageIndex,
        int pageSize,
        string? cocktailName = null,
        int? ingredientsExactCount = null,
        List<int>? ingredientsList = null,
        string? sort = null
    );
}
