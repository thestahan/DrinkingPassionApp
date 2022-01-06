using Core.Entities;
using System.Linq;

namespace Core.Specifications.CocktailsLists
{
    public class CocktailsListWithCocktailExists : BaseSpecification<CocktailsList>
    {
        public CocktailsListWithCocktailExists(string uniqueLink, int cocktailId) :
            base(l => l.UniqueLink == uniqueLink && l.Cocktails.Any(c => c.Id == cocktailId))
        {
        }
    }
}
