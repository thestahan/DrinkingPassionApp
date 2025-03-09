using FluentValidation;

namespace DrinkingPassion.Api.Dtos.Products;

public class ProductToAddDto : ICommandDto
{
    public required bool IsPrivate { get; set; }
    public required string Name { get; set; }
    public required int ProductTypeId { get; set; }
    public required int ProductUnitId { get; set; }
}

public class ProductToAddDtoValidator : AbstractValidator<ProductToAddDto>
{
    public ProductToAddDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, 60);
        RuleFor(x => x.ProductTypeId).GreaterThan(0);
        RuleFor(x => x.ProductUnitId).GreaterThan(0);
    }
}
