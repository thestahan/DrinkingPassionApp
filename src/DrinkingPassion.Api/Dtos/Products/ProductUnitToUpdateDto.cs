using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Products
{
    public class ProductUnitToUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(maximumLength: 30, MinimumLength = 2)]
        [Required]
        public string Name { get; set; }

        [StringLength(maximumLength: 15, MinimumLength = 1)]
        [Required]
        public string Abbreviation { get; set; }
    }
}
