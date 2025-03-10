using DrinkingPassion.Shared.Models;

namespace DrinkingPassion.Api.Dtos.Products;

public class ProductTypeToReturnDto : IQueryDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
}
