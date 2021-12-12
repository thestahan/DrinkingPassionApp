using Core.Entities;

namespace Core.Specifications
{
    public class IngredientIsPartOfAnyCocktailByPrivacySpec : BaseSpecification<Ingredient>
    {
        public IngredientIsPartOfAnyCocktailByPrivacySpec(int ingredientId, string authorId, bool isPrivate)
            : base (i => i.ProductId == ingredientId && i.Product.IsPrivate == isPrivate && i.Product.AuthorId == authorId)
        {
        }
    }
}
