namespace DrinkingPassion.Api.Dtos.Ingredients;

public class IngredientToReturnDto : IQueryDto
{
    public required double Amount { get; set; }
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int ProductId { get; set; }
    public required string Unit { get; set; }
}
