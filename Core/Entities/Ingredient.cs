using System.Collections.Generic;

namespace Core.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public Product Product { get; set; }
        public ICollection<Cocktail> Cocktails { get; set; }
    }
}
