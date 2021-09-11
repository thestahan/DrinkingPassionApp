using Core.Entities;

namespace Core.Specifications
{
    public class CocktailWithIngredientsSpecification : BaseSpecification<Cocktail>
    {
        public CocktailWithIngredientsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude("Ingredients.Product");
            AddInclude(x => x.BaseProduct);
        }
    }
}
