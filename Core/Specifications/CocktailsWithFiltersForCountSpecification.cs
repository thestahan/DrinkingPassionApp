using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class CocktailsWithFiltersForCountSpecification : BaseSpecification<Cocktail>
    {
        public CocktailsWithFiltersForCountSpecification(CocktailSpecParams cocktailParams, bool isPrivate, string authorId = "")
            : base(x =>
                (string.IsNullOrEmpty(cocktailParams.CocktailName) || x.Name.ToLower().Contains(cocktailParams.CocktailName)) &&
                (!cocktailParams.ProductId.HasValue || x.Ingredients.Any(y => y.ProductId == cocktailParams.ProductId)) &&
                (string.IsNullOrEmpty(authorId) || x.AuthorId == authorId) &&
                (cocktailParams.IngredientsExactCount == 0 || x.IngredientsCount == cocktailParams.IngredientsExactCount) &&
                x.IsPrivate == isPrivate
            )
        {
        }
    }
}
