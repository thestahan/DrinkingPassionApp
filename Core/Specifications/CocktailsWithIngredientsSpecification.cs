using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class CocktailsWithIngredientsSpecification : BaseSpecification<Cocktail>
    {
        public CocktailsWithIngredientsSpecification(CocktailSpecParams cocktailParams)
            : base(x =>
                (string.IsNullOrEmpty(cocktailParams.Search) || x.Name.ToLower().Contains(cocktailParams.Search)) &&
                (!cocktailParams.ProductId.HasValue || x.Ingredients.Any(y => y.ProductId == cocktailParams.ProductId))
            )
        {
            AddInclude("Ingredients.Product");
            AddOrderBy(x => x.Name);
            ApplyPaging(cocktailParams.PageSize * (cocktailParams.PageIndex - 1), cocktailParams.PageSize);

            if (!string.IsNullOrEmpty(cocktailParams.Sort))
            {
                switch (cocktailParams.Sort)
                {
                    case "nameDesc":
                        AddOrderByDescending(x => x.Name);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
        }

        public CocktailsWithIngredientsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude("Ingredients.Product");
        }
    }
}
