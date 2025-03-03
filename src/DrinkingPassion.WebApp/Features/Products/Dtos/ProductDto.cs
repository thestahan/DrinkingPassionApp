namespace DrinkingPassion.WebApp.Features.Products.Dtos;

public record ProductDto
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string ProductType { get; init; }
    public required int ProductTypeId { get; init; }
    public required string ProductUnit { get; init; }
    public string? ProductUnitAbbreviation { get; init; }
    public required int ProductUnitId { get; init; }
}
