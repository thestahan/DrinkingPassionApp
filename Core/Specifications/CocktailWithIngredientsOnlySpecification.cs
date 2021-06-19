using Core.Entities;

namespace Core.Specifications
{
    public class CocktailWithIngredientsOnlySpecification : BaseSpecification<Cocktail>
    {
        public CocktailWithIngredientsOnlySpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Ingredients);
        }
    }
}
