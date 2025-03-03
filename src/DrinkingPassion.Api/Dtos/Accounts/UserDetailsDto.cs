using DrinkingPassion.Api.Core.Entities.Enums;

namespace DrinkingPassion.Api.Dtos.Accounts
{
    public class UserDetailsDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public BartenderType BartenderType { get; set; }
    }
}
