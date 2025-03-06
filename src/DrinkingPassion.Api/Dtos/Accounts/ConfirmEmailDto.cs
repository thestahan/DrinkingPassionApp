namespace DrinkingPassion.Api.Dtos.Accounts
{
    public class ConfirmEmailDto
    {
        public required string Token { get; set; }
        public required string Email { get; set; }
    }
}
