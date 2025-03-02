using Core.Entities;

namespace Core.Specifications.CocktailsLists
{
    public class CocktailsListForUserById : BaseSpecification<CocktailsList>
    {
        public CocktailsListForUserById(int id) : base(l => l.Id == id)
        {
        }
    }
}
