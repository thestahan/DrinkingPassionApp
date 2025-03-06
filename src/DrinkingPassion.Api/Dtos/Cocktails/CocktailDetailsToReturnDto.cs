using DrinkingPassion.Api.Dtos.Ingredients;
using System.Collections.Generic;

namespace DrinkingPassion.Api.Dtos.Cocktails
{
    public class CocktailDetailsToReturnDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Picture { get; set; }
        public string? Description { get; set; }
        public string? PreparationInstruction { get; set; }
        public required string BaseIngredient { get; set; }
        public required int IngredientsCount { get; set; }
        public ICollection<IngredientToReturnDto> Ingredients { get; set; } = [];
    }
}
