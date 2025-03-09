using FluentValidation;

namespace DrinkingPassion.Api.Dtos.Products;

public class ProductUnitToUpdateDto : ICommandDto
{
    public required string Abbreviation { get; set; }
    public int Id { get; set; }
    public required string Name { get; set; }
}

public class ProductUnitToUpdateDtoValidator : AbstractValidator<ProductUnitToUpdateDto>
{
    public ProductUnitToUpdateDtoValidator()
    {
        RuleFor(x => x.Abbreviation).NotEmpty().Length(1, 15);
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().Length(2, 30);
    }
}
