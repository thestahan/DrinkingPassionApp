using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Cocktails
{
    public class CocktailInfoToUpdateDto
    {
        public required int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 60, MinimumLength = 2)]
        public required string Name { get; set; }

        public string? Picture { get; set; }

        public string? Description { get; set; }

        public string? PreparationInstruction { get; set; }
    }
}
