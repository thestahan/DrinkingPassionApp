using API.Dtos.Ingredients;
using System.Collections.Generic;

namespace API.Dtos.Cocktails
{
    public class CocktailToAddDto
    {
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string PreparationInstruction { get; set; }
        public ICollection<IngredientToAddDto> Ingredients { get; set; }
    }
}
