using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Ingredients
{
    public class IngredientToAddDto
    {
        [Required]
        [Range(.1, 1000)]
        public required double Amount { get; set; }

        [Required]
        public required int ProductId { get; set; }
    }
}
