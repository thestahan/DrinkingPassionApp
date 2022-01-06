using Core.Entities;

namespace Core.Specifications.CocktailsLists
{
    public class CocktailsListByLinkWithCocktails : BaseSpecification<CocktailsList>
    {
        public CocktailsListByLinkWithCocktails(string link) : base(l => l.UniqueLink == link)
        {
            AddInclude("Cocktails.BaseProduct");
        }
    }
}
