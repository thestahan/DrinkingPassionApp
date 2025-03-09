using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DrinkingPassion.Api.Dtos.Cocktails;

public class CocktailToManageDto : ICommandDto
{
    public int BaseProductId { get; set; }
    public string? Description { get; set; }
    public int? Id { get; set; }
    public required string Ingredients { get; set; }
    public bool IsPrivate { get; set; }
    public required string Name { get; set; }
    public IFormFile? Picture { get; set; }
    public string? PreparationInstruction { get; set; }
}

public class CocktailToManageDtoValidator : AbstractValidator<CocktailToManageDto>
{
    public CocktailToManageDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(2, 60)
            .WithMessage("Name must be between 2 and 60 characters");

        RuleFor(x => x.Ingredients)
            .NotEmpty()
            .WithMessage("Ingredients are required");

        RuleFor(x => x.Picture)
            .Custom((picture, context) =>
            {
                if (picture == null)
                {
                    return;
                }

                // Max file size validation (5MB)
                if (picture.Length > 5 * 1024 * 1024)
                {
                    context.AddFailure("Picture", "File size exceeds the 5MB limit");
                }

                // Allowed file extensions validation
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = System.IO.Path.GetExtension(picture.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    context.AddFailure("Picture", $"File extension {fileExtension} is not allowed. Allowed extensions: {string.Join(", ", allowedExtensions)}");
                }
            });
    }
}
