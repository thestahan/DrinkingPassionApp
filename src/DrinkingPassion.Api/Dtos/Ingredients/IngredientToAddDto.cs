using DrinkingPassion.Shared.Models;
using FluentValidation;

namespace DrinkingPassion.Api.Dtos.Ingredients;

public class IngredientToAddDto : ICommandDto
{
    public required double Amount { get; set; }
    public required int ProductId { get; set; }
}

public class IngredientToAddDtoValidator : AbstractValidator<IngredientToAddDto>
{
    public IngredientToAddDtoValidator()
    {
        RuleFor(x => x.Amount).InclusiveBetween(.1, 1000);
        RuleFor(x => x.ProductId).GreaterThan(0);
    }
}
