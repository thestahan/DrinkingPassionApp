using Core.Entities;

namespace Core.Specifications
{
    public class CocktailIngredientExistsSpecification : BaseSpecification<Ingredient>
    {
        public CocktailIngredientExistsSpecification(int cocktailId, int igredientId) 
            : base(x => x.Id == igredientId && x.CocktailId == cocktailId)
        {
        }
    }
}
