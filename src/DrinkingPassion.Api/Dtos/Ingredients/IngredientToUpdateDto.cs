using DrinkingPassion.Shared.Models;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Ingredients;

public class IngredientToUpdateDto : ICommandDto
{
    [Required]
    [Range(.1, 1000)]
    public required double Amount { get; set; }

    public required int Id { get; set; }
}

public class IngredientToUpdateDtoValidator : AbstractValidator<IngredientToUpdateDto>
{
    public IngredientToUpdateDtoValidator()
    {
        RuleFor(x => x.Amount).InclusiveBetween(.1, 1000);
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
