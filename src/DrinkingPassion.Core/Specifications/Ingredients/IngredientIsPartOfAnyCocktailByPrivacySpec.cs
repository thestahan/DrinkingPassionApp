using DrinkingPassion.Api.Core.Entities;

namespace DrinkingPassion.Api.Core.Specifications.Ingredients
{
    public class IngredientIsPartOfAnyCocktailByPrivacySpec : BaseSpecification<Ingredient>
    {
        public IngredientIsPartOfAnyCocktailByPrivacySpec(int ingredientId, string authorId, bool isPrivate)
            : base(i => i.ProductId == ingredientId && i.Product.IsPrivate == isPrivate && i.Product.AuthorId == authorId)
        {
        }
    }
}
