using FluentValidation;

namespace DrinkingPassion.Api.Dtos.Accounts;

public class UserChangePasswordDto : ICommandDto
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}

public class UserChangePasswordDtoValidator : AbstractValidator<UserChangePasswordDto>
{
    public UserChangePasswordDtoValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .MinimumLength(8);
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8);
    }
}
