using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Products;

public class ProductToUpdateDto : ICommandDto
{
    [Required]
    public required int Id { get; set; }

    [Required]
    [StringLength(maximumLength: 60, MinimumLength = 2)]
    public required string Name { get; set; }

    [Required]
    public required int ProductTypeId { get; set; }

    [Required]
    public required int ProductUnitId { get; set; }
}

public class ProductToUpdateDtoValidator : AbstractValidator<ProductToUpdateDto>
{
    public ProductToUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, 60);
        RuleFor(x => x.ProductTypeId).GreaterThan(0);
        RuleFor(x => x.ProductUnitId).GreaterThan(0);
    }
}
