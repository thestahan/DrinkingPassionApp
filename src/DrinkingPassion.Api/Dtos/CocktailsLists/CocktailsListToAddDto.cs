using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.CocktailsLists;

public class CocktailsListToAddDto
{

    public required IReadOnlyList<int> Cocktails { get; set; } = [];
    public int? Id { get; set; }

    [Required]
    public required string Name { get; set; }
}
