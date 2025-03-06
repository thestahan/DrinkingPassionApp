namespace DrinkingPassion.Api.Dtos.Roles
{
    public class AddUserToRoleDto
    {
        public required string UserEmail { get; set; }
        public required string Role { get; set; }
    }
}
