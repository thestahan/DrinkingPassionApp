using System.Collections.Generic;

namespace DrinkingPassion.Api.Dtos.Cocktails
{
    public class CocktailDetailsToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string PreparationInstruction { get; set; }
        public string BaseIngredient { get; set; }
        public int IngredientsCount { get; set; }
        public ICollection<DrinkingPassion.Api.Dtos.Ingredients.IngredientToReturnDto> Ingredients { get; set; }
    }
}
