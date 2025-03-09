using DrinkingPassion.Api.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Cocktails;

public class CocktailToManageDto : ICommandDto
{
    public int BaseProductId { get; set; }

    public string? Description { get; set; }
    public int? Id { get; set; }

    [Required]
    public required string Ingredients { get; set; }

    public bool IsPrivate { get; set; }

    [Required]
    [StringLength(maximumLength: 60, MinimumLength = 2)]
    public required string Name { get; set; }

    [MaxFileSize(5 * 1024 * 1014)]
    [AllowedFileExtensions([".jpg", ".jpeg", ".png"])]
    public IFormFile? Picture { get; set; }

    public string? PreparationInstruction { get; set; }
}
