namespace DrinkingPassion.Api.Core.Specifications.CocktailsLists
{
    public class CocktailsListsForUser : BaseSpecification<DrinkingPassion.Api.Core.Entities.CocktailsList>
    {
        public CocktailsListsForUser(string authorId) : base(l => l.AuthorId == authorId)
        {
            AddOrderBy(l => l.Name);
        }
    }
}
