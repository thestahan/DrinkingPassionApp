using Core.Entities;

namespace Core.Specifications
{
    public class IngredientsCountForCocktailSpecification : BaseSpecification<Ingredient>
    {
        public IngredientsCountForCocktailSpecification(int cocktailId)
            : base(x => x.CocktailId == cocktailId)
        {
        }
    }
}
