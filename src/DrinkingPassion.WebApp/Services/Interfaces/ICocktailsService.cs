using DrinkingPassion.WebApp.Features.Cocktails.Dtos;
using DrinkingPassion.WebApp.Shared;

namespace DrinkingPassion.WebApp.Services.Interfaces;

public interface ICocktailsService
{
    public Task<CocktailDetails?> GetCocktailDetails(int id);

    public Task<Pagination<CocktailDto>?> GetPublicCocktails(int pageIndex);
}
