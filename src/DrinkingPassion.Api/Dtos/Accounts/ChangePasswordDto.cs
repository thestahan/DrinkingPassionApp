using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Accounts
{
    public class ChangePasswordDto
    {
        [Required]
        [MinLength(8)]
        public required string CurrentPassword { get; set; }

        [Required]
        [MinLength(8)]
        public required string NewPassword { get; set; }
    }
}
