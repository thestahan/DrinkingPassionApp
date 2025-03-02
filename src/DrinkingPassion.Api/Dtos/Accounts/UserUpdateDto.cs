using Core.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Accounts
{
    public class UserUpdateDto
    {
        [StringLength(60, MinimumLength = 2)]
        public string FirstName { get; set; }

        [StringLength(60, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(60, MinimumLength = 4)]
        public string Email { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string DisplayName { get; set; }

        [Required]
        public BartenderType BartenderType { get; set; }
    }
}
