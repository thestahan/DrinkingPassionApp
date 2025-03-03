namespace DrinkingPassion.Api.Core.Entities
{
    public class Ingredient : BaseEntity
    {
        public double Amount { get; set; }
        public DrinkingPassion.Api.Core.Entities.Product Product { get; set; }
        public int ProductId { get; set; }
        public DrinkingPassion.Api.Core.Entities.Cocktail Cocktail { get; set; }
        public int CocktailId { get; set; }
    }
}