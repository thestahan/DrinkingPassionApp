using Core.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        [StringLength(60, MinimumLength = 4)]
        public string Email { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string DisplayName { get; set; }

        [Required]
        public string Password { get; set; }

        public BartenderType BartenderType { get; set; }
    }
}
