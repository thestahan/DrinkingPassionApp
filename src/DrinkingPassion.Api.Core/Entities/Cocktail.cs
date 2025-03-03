using System.Collections.Generic;

namespace DrinkingPassion.Api.Core.Entities
{
    public class Cocktail : BaseEntity
    {
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string PreparationInstruction { get; set; }
        public bool IsPrivate { get; set; }
        public string AuthorId { get; set; }
        public DrinkingPassion.Api.Core.Entities.Identity.AppUser Author { get; set; }
        public int? BaseProductId { get; set; }
        public Product BaseProduct { get; set; }
        public int IngredientsCount { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<CocktailsList> CocktailsLists { get; set; }
    }
}
