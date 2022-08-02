using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.CocktailsLists
{
    public class CocktailsListToAddDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IReadOnlyList<int> Cocktails { get; set; } = new List<int>();
    }
}
