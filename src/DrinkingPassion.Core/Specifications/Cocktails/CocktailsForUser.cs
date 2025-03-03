namespace DrinkingPassion.Api.Core.Specifications.Cocktails
{
    public class CocktailsForUser : BaseSpecification<DrinkingPassion.Api.Core.Entities.Cocktail>
    {
        public CocktailsForUser(string userId) : 
            base(c => !c.IsPrivate || c.AuthorId == userId)
        {
            AddOrderBy(c => c.Name);
        }
    }
}
