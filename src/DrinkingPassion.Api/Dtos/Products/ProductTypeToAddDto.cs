using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Products;

public class ProductTypeToAddDto : ICommandDto
{
    [StringLength(maximumLength: 60, MinimumLength = 3)]
    [Required]
    public required string Name { get; set; }
}
