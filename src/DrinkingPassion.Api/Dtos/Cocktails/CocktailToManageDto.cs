using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Cocktails
{
    public class CocktailToManageDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 60, MinimumLength = 2)]
        public string Name { get; set; }

        [DrinkingPassion.Api.ValidationAttributes.MaxFileSizeAttribute(5 * 1024 * 1014)]
        [DrinkingPassion.Api.ValidationAttributes.AllowedFileExtensionsAttribute(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile Picture { get; set; }

        public string Description { get; set; }

        public string PreparationInstruction { get; set; }

        public int BaseProductId { get; set; }

        [Required]
        public string Ingredients { get; set; }

        public bool IsPrivate { get; set; }
    }
}
