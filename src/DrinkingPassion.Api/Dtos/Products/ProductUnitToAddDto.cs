using DrinkingPassion.Shared.Models;
using FluentValidation;

namespace DrinkingPassion.Api.Dtos.Products;

public class ProductUnitToAddDto : ICommandDto
{
    public required string Abbreviation { get; set; }
    public required string Name { get; set; }
}

public class ProductUnitToAddDtoValidator : AbstractValidator<ProductUnitToAddDto>
{
    public ProductUnitToAddDtoValidator()
    {
        RuleFor(x => x.Abbreviation).NotEmpty().Length(1, 15);
        RuleFor(x => x.Name).NotEmpty().Length(2, 30);
    }
}
