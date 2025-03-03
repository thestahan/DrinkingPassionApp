using System;
using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Ingredients
{
    public class IngredientToUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [Range(.1, 1000)]
        public double Amount { get; set; }
    }
}
