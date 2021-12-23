using Core.Entities;

namespace Core.Specifications.CocktailsLists
{
    public class CocktailsListForUserByIdWithCocktails : BaseSpecification<CocktailsList>
    {
        public CocktailsListForUserByIdWithCocktails(string authorId, int id) : base(l => l.Id == id && l.AuthorId == authorId)
        {
            AddInclude(l => l.Cocktails);
        }
    }
}
