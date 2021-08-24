using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Accounts
{
    public class ChangePasswordDto
    {
        [Required]
        [MinLength(8)]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }
    }
}
