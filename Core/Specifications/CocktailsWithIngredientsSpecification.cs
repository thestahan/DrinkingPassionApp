using Core.Entities;

namespace Core.Specifications
{
    public class CocktailsWithIngredientsSpecification : BaseSpecification<Cocktail>
    {
        public CocktailsWithIngredientsSpecification()
        {
            AddInclude("Ingredients.Product");
        }

        public CocktailsWithIngredientsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude("Ingredients.Product");
        }
    }
}
