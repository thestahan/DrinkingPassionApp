namespace DrinkingPassion.Api.Dtos.Ingredients;

public class IngredientAsPartOfCocktailDto : IQueryDto
{
    public required int Id { get; set; }
    public required bool IsPrivate { get; set; }
}
