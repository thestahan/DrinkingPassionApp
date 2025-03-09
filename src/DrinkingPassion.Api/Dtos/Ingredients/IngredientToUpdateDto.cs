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
