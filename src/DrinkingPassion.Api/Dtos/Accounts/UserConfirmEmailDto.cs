namespace DrinkingPassion.Api.Dtos.Accounts;

public class UserConfirmEmailDto : ICommandDto
{
    public required string Email { get; set; }
    public required string Token { get; set; }
}
