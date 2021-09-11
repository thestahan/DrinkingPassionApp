using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Products
{
    public class ProductToAddDto
    {
        [Required]
        [StringLength(maximumLength: 60, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public int ProductUnitId { get; set; }

        [Required]
        public int ProductTypeId { get; set; }
    }
}