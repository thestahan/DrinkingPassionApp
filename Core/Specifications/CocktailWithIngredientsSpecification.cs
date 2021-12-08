using Core.Entities;

namespace Core.Specifications
{
    public class CocktailWithIngredientsSpecification : BaseSpecification<Cocktail>
    {
        public CocktailWithIngredientsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude("Ingredients.Product.ProductUnit");
            AddInclude(x => x.BaseProduct);
        }
    }
}
