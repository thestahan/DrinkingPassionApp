using DrinkingPassion.Shared.Models;
using FluentValidation;

namespace DrinkingPassion.Api.Dtos.Products;

public class ProductTypeToUpdateDto : ICommandDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public class ProductTypeToUpdateDtoValidator : AbstractValidator<ProductTypeToUpdateDto>
{
    public ProductTypeToUpdateDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().Length(3, 60);
    }
}
