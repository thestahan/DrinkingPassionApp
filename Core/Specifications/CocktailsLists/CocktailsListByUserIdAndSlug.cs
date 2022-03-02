using Core.Entities;

namespace Core.Specifications.CocktailsLists
{
    public class CocktailsListByUserIdAndSlug : BaseSpecification<CocktailsList>
    {
        public CocktailsListByUserIdAndSlug(string userId, string slug) : base(l => l.AuthorId == userId && l.Slug == slug)
        {
            AddInclude("Cocktails.BaseProduct");
        }
    }
}
