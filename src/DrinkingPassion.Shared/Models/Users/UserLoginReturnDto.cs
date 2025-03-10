namespace DrinkingPassion.Shared.Models.Users;

public class UserLoginReturnDto : IQueryDto
{
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public required ICollection<string> Roles { get; set; }
    public required string Token { get; set; }
    public required string TokenExpiration { get; set; }
}
