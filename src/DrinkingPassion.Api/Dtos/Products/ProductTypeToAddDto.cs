using DrinkingPassion.Shared.Models;
using FluentValidation;

namespace DrinkingPassion.Api.Dtos.Products;

public class ProductTypeToAddDto : ICommandDto
{
    public required string Name { get; set; }
}

public class ProductTypeToAddDtoValidator : AbstractValidator<ProductTypeToAddDto>
{
    public ProductTypeToAddDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(3, 60);
    }
}
