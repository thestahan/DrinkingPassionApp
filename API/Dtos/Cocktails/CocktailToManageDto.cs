using API.Dtos.Ingredients;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Cocktails
{
    public class CocktailToManageDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 60, MinimumLength = 2)]
        public string Name { get; set; }

        public string Picture { get; set; }

        public string Description { get; set; }

        public string PreparationInstruction { get; set; }

        public int BaseProductId { get; set; }

        [Required]
        public ICollection<IngredientToAddDto> Ingredients { get; set; }
    }
}
