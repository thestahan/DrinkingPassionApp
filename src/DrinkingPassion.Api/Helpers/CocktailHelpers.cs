using DrinkingPassion.Api.Core.Entities;

namespace DrinkingPassion.Api.Helpers
{
    public class CocktailHelpers
    {
        public static Cocktail ApplyCocktailInfoChangesToCocktail(Dtos.Cocktails.CocktailInfoToUpdateDto toUpdate, Cocktail cocktail)
        {
            cocktail.Name = toUpdate.Name;
            cocktail.Picture = toUpdate.Picture;
            cocktail.Description = toUpdate.Description;
            cocktail.PreparationInstruction = toUpdate.PreparationInstruction;

            return cocktail;
        }
    }
}
