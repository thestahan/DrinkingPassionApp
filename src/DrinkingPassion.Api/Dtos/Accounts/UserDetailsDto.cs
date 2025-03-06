using DrinkingPassion.Api.Core.Entities.Enums;

namespace DrinkingPassion.Api.Dtos.Accounts
{
    public class UserDetailsDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
        public required string DisplayName { get; set; }
        public required BartenderType BartenderType { get; set; }
    }
}
