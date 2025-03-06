namespace DrinkingPassion.Api.Dtos.Cocktails
{
    public class CocktailToReturnDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Picture { get; set; }
        public string? BaseIngredient { get; set; }
        public int IngredientsCount { get; set; }
        public bool IsPrivate { get; set; }
    }
}
