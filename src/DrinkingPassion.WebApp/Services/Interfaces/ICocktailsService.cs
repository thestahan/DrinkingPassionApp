using DrinkingPassion.Shared.Models;
using DrinkingPassion.Shared.Models.Cocktails;

namespace DrinkingPassion.WebApp.Services.Interfaces;

public interface ICocktailsService
{
    public Task<CocktailDetailsToReturnDto?> GetCocktailDetails(int id);

    public Task<Pagination<CocktailToReturnDto>?> GetPublicCocktails(int pageIndex);
}
