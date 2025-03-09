namespace DrinkingPassion.Api.Dtos.Accounts;

public class UserRegisterReturnDto : IQueryDto
{
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
}
