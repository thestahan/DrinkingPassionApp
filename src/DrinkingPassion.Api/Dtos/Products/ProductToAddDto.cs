using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Products;

public class ProductToAddDto : ICommandDto
{
    [Required]
    public required bool IsPrivate { get; set; }

    [Required]
    [StringLength(maximumLength: 60, MinimumLength = 2)]
    public required string Name { get; set; }

    [Required]
    public required int ProductTypeId { get; set; }

    [Required]
    public required int ProductUnitId { get; set; }
}
