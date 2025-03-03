namespace DrinkingPassion.WebApp.Features.Cocktails.Dtos;

public class CocktailDto
{
    public string BaseIngredient { get; set; }
    public int Id { get; set; }
    public int IngredientsCount { get; set; }
    public bool IsPrivate { get; set; }
    public string Name { get; set; }
    public string? Picture { get; set; }
}
