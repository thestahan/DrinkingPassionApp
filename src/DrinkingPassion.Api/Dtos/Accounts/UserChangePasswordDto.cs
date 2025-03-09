using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Accounts;

public class UserChangePasswordDto : ICommandDto
{
    [Required]
    [MinLength(8)]
    public required string CurrentPassword { get; set; }

    [Required]
    [MinLength(8)]
    public required string NewPassword { get; set; }
}
