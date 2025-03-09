using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Products;

public class ProductUnitToAddDto : ICommandDto
{
    [StringLength(maximumLength: 15, MinimumLength = 1)]
    [Required]
    public required string Abbreviation { get; set; }

    [StringLength(maximumLength: 30, MinimumLength = 2)]
    [Required]
    public required string Name { get; set; }
}
