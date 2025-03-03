namespace DrinkingPassion.WebApp.Features.Cocktails.Dtos;

public class CocktailDetails
{
    public string BaseIngredient { get; init; } = default!;
    public string? Description { get; init; }
    public int Id { get; init; }

    public ICollection<Ingredient> Ingredients { get; init; } = [];
    public int IngredientsCount { get; init; }
    public string Name { get; init; } = default!;
    public string? Picture { get; init; }
    public string? PreparationInstruction { get; init; }

    public class Ingredient
    {
        public double Amount { get; init; }
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public int ProductId { get; init; }
        public string Unit { get; init; } = default!;
    }
}
