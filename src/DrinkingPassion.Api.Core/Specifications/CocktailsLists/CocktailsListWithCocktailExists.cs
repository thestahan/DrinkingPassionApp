using System.Linq;

namespace DrinkingPassion.Api.Core.Specifications.CocktailsLists
{
    public class CocktailsListWithCocktailExists : BaseSpecification<DrinkingPassion.Api.Core.Entities.CocktailsList>
    {
        public CocktailsListWithCocktailExists(string authorId, string slug, int cocktailId) :
            base(l => l.AuthorId == authorId && l.Slug == slug && l.Cocktails.Any(c => c.Id == cocktailId))
        {
        }
    }
}
