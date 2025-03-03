namespace DrinkingPassion.WebApp.Features.Products.Dtos;

public record ProductToAddDto
{
    public required bool IsPrivate { get; init; }
    public required string Name { get; init; }
    public required int ProductTypeId { get; init; }
    public required int ProductUnitId { get; init; }
}
