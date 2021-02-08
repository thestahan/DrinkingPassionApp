using System.Collections.Generic;

namespace Core.Entities
{
    public class Cocktail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string PreparationInstruction { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
