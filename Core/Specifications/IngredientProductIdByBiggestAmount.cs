using Core.Entities;

namespace Core.Specifications
{
    public class IngredientProductIdByBiggestAmount : BaseSpecification<Ingredient>
    {
        public IngredientProductIdByBiggestAmount(int cocktailId)
            : base(x => x.CocktailId == cocktailId)
        {
            AddOrderByDescending(x => x.Amount);
            AddSelect(x => x.ProductId);
        }
    }
}
