using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Products
{
    public class ProductUnitToAddDto
    {
        [StringLength(maximumLength: 30, MinimumLength = 2)]
        [Required]
        public string Name { get; set; }

        [StringLength(maximumLength: 15, MinimumLength = 1)]
        [Required]
        public string Abbreviation { get; set; }
    }
}
