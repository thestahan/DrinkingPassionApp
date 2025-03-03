namespace DrinkingPassion.WebApp.Features.Products.Dtos;

public record ProductToUpdateDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int ProductTypeId { get; set; }
    public required int ProductUnitId { get; set; }
}
