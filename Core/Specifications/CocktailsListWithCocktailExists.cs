using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class CocktailsListWithCocktailExists : BaseSpecification<CocktailsList>
    {
        public CocktailsListWithCocktailExists(int listId, int cocktailId) : 
            base(l => l.Id == listId && l.Cocktails.Any(c => c.Id == cocktailId))
        {
        }
    }
}
