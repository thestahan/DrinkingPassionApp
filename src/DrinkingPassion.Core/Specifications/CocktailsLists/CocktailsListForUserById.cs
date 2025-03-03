using DrinkingPassion.Api.Core.Entities;

namespace DrinkingPassion.Api.Core.Specifications.CocktailsLists
{
    public class CocktailsListForUserById : BaseSpecification<CocktailsList>
    {
        public CocktailsListForUserById(int id) : base(l => l.Id == id)
        {
        }
    }
}
