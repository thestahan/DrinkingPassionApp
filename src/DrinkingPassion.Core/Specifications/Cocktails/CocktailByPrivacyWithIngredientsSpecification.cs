namespace DrinkingPassion.Api.Core.Specifications.Cocktails
{
    public class CocktailByPrivacyWithIngredientsSpecification : BaseSpecification<DrinkingPassion.Api.Core.Entities.Cocktail>
    {
        public CocktailByPrivacyWithIngredientsSpecification(int id, bool isPrivate) :
            base(x => x.Id == id && x.IsPrivate == isPrivate)
        {
            AddInclude("Ingredients.Product");
            AddInclude(x => x.BaseProduct);
        }
    }
}
