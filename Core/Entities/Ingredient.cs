namespace Core.Entities
{
    public class Ingredient : BaseEntity
    {
        public double Amount { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Cocktail Cocktail { get; set; }
        public int CocktailId { get; set; }
    }
}