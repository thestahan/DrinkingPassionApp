using Core.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class CocktailsList : BaseEntity
    {
        public Guid UniqueLink { get; set; }
        public string Name { get; set; }
        public AppUser Author { get; set; }
        public string AuthorId { get; set; }
        public IReadOnlyList<Cocktail> Cocktails { get; set; }
    }
}
