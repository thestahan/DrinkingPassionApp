using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class CocktailsWithFiltersForCountSpecification : BaseSpecification<Cocktail>
    {
        public CocktailsWithFiltersForCountSpecification(CocktailSpecParams cocktailParams)
            : base(x =>
                (string.IsNullOrEmpty(cocktailParams.Search) || x.Name.ToLower().Contains(cocktailParams.Search)) &&
                (!cocktailParams.ProductId.HasValue || x.Ingredients.Any(y => y.ProductId == cocktailParams.ProductId))
            )
        {
        }
    }
}
