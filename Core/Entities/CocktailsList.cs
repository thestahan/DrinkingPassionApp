using Core.Entities.Identity;
using System.Collections.Generic;

namespace Core.Entities
{
    public class CocktailsList : BaseEntity
    {
        public string UniqueLink { get; set; }
        public string Name { get; set; }
        public AppUser Author { get; set; }
        public string AuthorId { get; set; }
        public ICollection<Cocktail> Cocktails { get; set; }
        public int CocktailsCount { get; set; }
    }
}
