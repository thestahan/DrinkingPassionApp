using System.Collections.Generic;

namespace API.Dtos
{
    public class CocktailToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string PreparationInstruction { get; set; }
        public ICollection<IngredientToReturnDto> Ingredients { get; set; }
    }
}
