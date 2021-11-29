using Core.Entities;

namespace Core.Specifications
{
    public class CocktailByPrivacyWithIngredientsSpecification : BaseSpecification<Cocktail>
    {
        public CocktailByPrivacyWithIngredientsSpecification(int id, bool isPrivate) : 
            base(x => x.Id == id && x.IsPrivate == isPrivate)
        {
            AddInclude("Ingredients.Product");
            AddInclude(x => x.BaseProduct);
        }
    }
}
