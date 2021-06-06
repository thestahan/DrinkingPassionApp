using API.Dtos.Cocktails;
using Core.Entities;
using System.Linq;

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
            //cocktail.BaseProductId = toUpdate.Ingredients.OrderByDescending(x => x.Amount)
            //    .Select(x => x.ProductId)
            //    .FirstOrDefault();

            return cocktail;
        }
    }
}
