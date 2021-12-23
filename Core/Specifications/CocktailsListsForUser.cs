using Core.Entities;

namespace Core.Specifications
{
    public class CocktailsListsForUser : BaseSpecification<CocktailsList>
    {
        public CocktailsListsForUser(string authorId) : base(l => l.AuthorId == authorId)
        {
            AddOrderBy(l => l.Name);
        }
    }
}
