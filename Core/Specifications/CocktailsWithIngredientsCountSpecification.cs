using Core.Entities;
using Core.Models;
using System.Linq;

namespace Core.Specifications
{
    public class CocktailsWithIngredientsCountSpecification : BaseSpecification<Cocktail>
    {
        public CocktailsWithIngredientsCountSpecification(CocktailSpecParams cocktailParams, bool isPrivate, string authorId = "")
            : base(x =>
                (string.IsNullOrEmpty(cocktailParams.Search) || x.Name.ToLower().Contains(cocktailParams.Search)) &&
                (!cocktailParams.ProductId.HasValue || x.Ingredients.Any(y => y.ProductId == cocktailParams.ProductId)) &&
                (string.IsNullOrEmpty(authorId) || x.AuthorId == authorId) &&
                x.IsPrivate == isPrivate
            )
        {
            AddInclude(x => x.BaseProduct);
            AddOrderBy(x => x.Name);
            ApplyPaging(cocktailParams.PageSize * (cocktailParams.PageIndex - 1), cocktailParams.PageSize);

            if (!string.IsNullOrEmpty(cocktailParams.Sort))
            {
                switch (cocktailParams.Sort)
                {
                    case CocktailsSort.NameDesc:
                        AddOrderByDescending(x => x.Name);
                        break;
                    case CocktailsSort.IngredientsAsc:
                        AddOrderBy(x => x.IngredientsCount);
                        break;
                    case CocktailsSort.IngredientsDesc:
                        AddOrderByDescending(x => x.IngredientsCount);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
        }
    }
}
