namespace DrinkingPassion.Api.Core.Specifications.CocktailsLists
{
    public class CocktailsListForUserByIdWithCocktails : BaseSpecification<DrinkingPassion.Api.Core.Entities.CocktailsList>
    {
        public CocktailsListForUserByIdWithCocktails(string authorId, int id) : base(l => l.Id == id && l.AuthorId == authorId)
        {
            AddInclude("Cocktails.BaseProduct");
        }
    }
}
