using DrinkingPassion.Shared.Models;
using FluentValidation;

namespace DrinkingPassion.Api.Dtos.Accounts;

public class UserChangeForgottenPasswordDto : ICommandDto
{
    public required string Email { get; set; }
    public required string NewPassword { get; set; }
    public required string Token { get; set; }
}

public class UserChangeForgottenPasswordDtoValidator : AbstractValidator<UserChangeForgottenPasswordDto>
{
    public UserChangeForgottenPasswordDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(6);
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
