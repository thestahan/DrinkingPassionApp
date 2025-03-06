namespace DrinkingPassion.Api.Dtos.Accounts
{
    public class ChangeForgottenPasswordDto
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }
}
