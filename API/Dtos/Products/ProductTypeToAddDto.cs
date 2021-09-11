using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Products
{
    public class ProductTypeToAddDto
    {
        [StringLength(maximumLength: 60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
    }
}
