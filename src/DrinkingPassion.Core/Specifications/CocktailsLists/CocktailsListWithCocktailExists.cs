using Core.Entities;
using System.Linq;

namespace Core.Specifications.CocktailsLists
{
    public class CocktailsListWithCocktailExists : BaseSpecification<CocktailsList>
    {
        public CocktailsListWithCocktailExists(string authorId, string slug, int cocktailId) :
            base(l => l.AuthorId == authorId && l.Slug == slug && l.Cocktails.Any(c => c.Id == cocktailId))
        {
        }
    }
}
