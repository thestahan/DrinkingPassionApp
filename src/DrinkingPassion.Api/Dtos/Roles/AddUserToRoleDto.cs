namespace DrinkingPassion.Api.Dtos.Roles;

public class AddUserToRoleDto : ICommandDto
{
    public required string Role { get; set; }
    public required string UserEmail { get; set; }
}
