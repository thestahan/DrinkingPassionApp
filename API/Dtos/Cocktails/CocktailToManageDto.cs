using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Cocktails
{
    public class CocktailToManageDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 60, MinimumLength = 2)]
        public string Name { get; set; }

        public IFormFile Picture { get; set; }

        public string Description { get; set; }

        public string PreparationInstruction { get; set; }

        public int BaseProductId { get; set; }

        [Required]
        public string Ingredients { get; set; }
    }
}
