namespace DrinkingPassion.Api.Core.Specifications.CocktailsLists
{
    public class CocktailsListByIdWithCocktails : BaseSpecification<DrinkingPassion.Api.Core.Entities.CocktailsList>
    {
        public CocktailsListByIdWithCocktails(int id) : base(l => l.Id == id)
        {
            AddInclude("Cocktails.BaseProduct");
        }
    }
}
