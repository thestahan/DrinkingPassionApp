using FluentValidation;

namespace DrinkingPassion.Api.Dtos.Roles;

public class AddUserToRoleDto : ICommandDto
{
    public required string Role { get; set; }
    public required string UserEmail { get; set; }
}

public class AddUserToRoleDtoValidator : AbstractValidator<AddUserToRoleDto>
{
    public AddUserToRoleDtoValidator()
    {
        RuleFor(x => x.Role).NotEmpty().Length(3, 30);
        RuleFor(x => x.UserEmail).NotEmpty().EmailAddress();
    }
}
