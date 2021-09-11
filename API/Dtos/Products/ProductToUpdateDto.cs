using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Products
{
    public class ProductToUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 60, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public int ProductUnitId { get; set; }

        [Required]
        public int ProductTypeId { get; set; }
    }
}
