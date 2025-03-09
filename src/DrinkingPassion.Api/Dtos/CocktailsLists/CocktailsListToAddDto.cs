using FluentValidation;
using System.Collections.Generic;

namespace DrinkingPassion.Api.Dtos.CocktailsLists;

public class CocktailsListToAddDto
{
    public required IReadOnlyList<int> Cocktails { get; set; } = [];
    public int? Id { get; set; }
    public required string Name { get; set; }
}

public class CocktailsListToAddDtoAbstractValidator : AbstractValidator<CocktailsListToAddDto>
{
    public CocktailsListToAddDtoAbstractValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(2, 60);
    }
}
