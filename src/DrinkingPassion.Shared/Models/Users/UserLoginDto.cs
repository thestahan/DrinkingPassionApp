using FluentValidation;

namespace DrinkingPassion.Shared.Models.Users;

public class UserLoginDto : ICommandDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class UserLoginDtoValidator : AbstractValidator<DrinkingPassion.Shared.Models.Users.UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
