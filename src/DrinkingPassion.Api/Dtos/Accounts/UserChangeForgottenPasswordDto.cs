namespace DrinkingPassion.Api.Dtos.Accounts;

public class UserChangeForgottenPasswordDto : ICommandDto
{
    public required string Email { get; set; }
    public required string NewPassword { get; set; }
    public required string Token { get; set; }
}
