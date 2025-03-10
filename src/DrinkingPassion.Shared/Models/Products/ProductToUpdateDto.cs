using FluentValidation;

namespace DrinkingPassion.Shared.Models.Products;

public class ProductToUpdateDto : ICommandDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int ProductTypeId { get; set; }
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
