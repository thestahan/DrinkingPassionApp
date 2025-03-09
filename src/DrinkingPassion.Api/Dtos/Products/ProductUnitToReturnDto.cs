namespace DrinkingPassion.Api.Dtos.Products;

public class ProductUnitToReturnDto : IQueryDto
{
    public required string Abbreviation { get; set; }
    public required int Id { get; set; }
    public required string Name { get; set; }
}
