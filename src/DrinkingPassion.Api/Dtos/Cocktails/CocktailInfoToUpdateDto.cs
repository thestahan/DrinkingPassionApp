using DrinkingPassion.Shared.Models;
using FluentValidation;

namespace DrinkingPassion.Api.Dtos.Cocktails;

public class CocktailInfoToUpdateDto : ICommandDto
{
    public string? Description { get; set; }
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Picture { get; set; }
    public string? PreparationInstruction { get; set; }
}

public class CocktailInfoToUpdateDtoValidator : AbstractValidator<CocktailInfoToUpdateDto>
{
    public CocktailInfoToUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, 60);
    }
}
