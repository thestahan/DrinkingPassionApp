using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Products
{
    public class ProductTypeToUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(maximumLength: 60, MinimumLength = 3)]
        [Required]
        public required string Name { get; set; }
    }
}
