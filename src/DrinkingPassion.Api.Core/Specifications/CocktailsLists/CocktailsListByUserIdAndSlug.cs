namespace DrinkingPassion.Api.Core.Specifications.CocktailsLists
{
    public class CocktailsListByUserIdAndSlug : BaseSpecification<DrinkingPassion.Api.Core.Entities.CocktailsList>
    {
        public CocktailsListByUserIdAndSlug(string userId, string slug) : base(l => l.AuthorId == userId && l.Slug == slug)
        {
            AddInclude("Cocktails.BaseProduct");
        }
    }
}
