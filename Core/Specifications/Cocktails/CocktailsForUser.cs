using Core.Entities;

namespace Core.Specifications.Cocktails
{
    public class CocktailsForUser : BaseSpecification<Cocktail>
    {
        public CocktailsForUser(string userId) : 
            base(c => !c.IsPrivate || c.AuthorId == userId)
        {
            AddOrderBy(c => c.Name);
        }
    }
}
