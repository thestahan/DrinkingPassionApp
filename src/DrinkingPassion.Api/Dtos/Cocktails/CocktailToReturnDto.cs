namespace DrinkingPassion.Api.Dtos.Cocktails;

public class CocktailToReturnDto : IQueryDto
{
    public string? BaseIngredient { get; set; }
    public required int Id { get; set; }
    public int IngredientsCount { get; set; }
    public bool IsPrivate { get; set; }
    public required string Name { get; set; }
    public string? Picture { get; set; }
}
