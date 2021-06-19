using API.Dtos.Cocktails;
using Core.Entities;

namespace API.Helpers
{
    public class CocktailUpdateHelpers
    {
        public static Cocktail ApplyCocktailInfoChangesToCocktail(CocktailInfoToUpdateDto toUpdate, Cocktail cocktail)
        {
            cocktail.Name = toUpdate.Name;
            cocktail.Picture = toUpdate.Picture;
            cocktail.Description = toUpdate.Description;
            cocktail.PreparationInstruction = toUpdate.PreparationInstruction;

            return cocktail;
        }
    }
}
