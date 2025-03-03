namespace DrinkingPassion.Api.Core.Specifications.Cocktails
{
    public class CocktailsWithFiltersForCountSpecification : BaseSpecification<DrinkingPassion.Api.Core.Entities.Cocktail>
    {
        public CocktailsWithFiltersForCountSpecification(CocktailSpecParams cocktailParams, bool isPrivate, string authorId = "")
            : base(x =>
                (string.IsNullOrEmpty(cocktailParams.CocktailName) || x.Name.ToLower().Contains(cocktailParams.CocktailName)) &&
                (!cocktailParams.ProductId.HasValue || x.BaseProductId == cocktailParams.ProductId) &&
                (string.IsNullOrEmpty(authorId) || x.AuthorId == authorId) &&
                (cocktailParams.IngredientsExactCount == 0 || x.IngredientsCount == cocktailParams.IngredientsExactCount) &&
                x.IsPrivate == isPrivate
            )
        {
        }
    }
}
