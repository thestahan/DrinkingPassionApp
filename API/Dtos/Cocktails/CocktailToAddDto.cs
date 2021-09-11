using API.Dtos.Ingredients;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Cocktails
{
    public class CocktailToAddDto
    {
        [Required]
        [StringLength(maximumLength: 60, MinimumLength = 2)]
        public string Name { get; set; }

        public string Picture { get; set; }

        public string Description { get; set; }

        public string PreparationInstruction { get; set; }

        [Required]
        public int BaseProductId { get; set; }

        [Required]
        public ICollection<IngredientToAddDto> Ingredients { get; set; }
    }
}
