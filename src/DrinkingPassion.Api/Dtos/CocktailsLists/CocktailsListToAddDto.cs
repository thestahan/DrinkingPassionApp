using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.CocktailsLists
{
    public class CocktailsListToAddDto
    {
        public int? Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public required IReadOnlyList<int> Cocktails { get; set; } = [];
    }
}
