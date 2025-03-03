using System.Collections.Generic;

namespace DrinkingPassion.Api.Core.Entities
{
    public class CocktailsList : BaseEntity
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public DrinkingPassion.Api.Core.Entities.Identity.AppUser Author { get; set; }
        public string AuthorId { get; set; }
        public ICollection<DrinkingPassion.Api.Core.Entities.Cocktail> Cocktails { get; set; }
        public int CocktailsCount { get; set; }
    }
}
