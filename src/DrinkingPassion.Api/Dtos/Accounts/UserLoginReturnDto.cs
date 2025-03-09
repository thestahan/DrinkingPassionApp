using System.Collections.Generic;

namespace DrinkingPassion.Api.Dtos.Accounts;

public class UserLoginReturnDto : IQueryDto
{
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public required ICollection<string> Roles { get; set; }
    public required string Token { get; set; }
    public required string TokenExpiration { get; set; }
}
