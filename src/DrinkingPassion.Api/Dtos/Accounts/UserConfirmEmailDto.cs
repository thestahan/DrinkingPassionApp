using DrinkingPassion.Shared.Models;
using FluentValidation;

namespace DrinkingPassion.Api.Dtos.Accounts;

public class UserConfirmEmailDto : ICommandDto
{
    public required string Email { get; set; }
    public required string Token { get; set; }
}

public class UserConfirmEmailDtoValidator : AbstractValidator<UserConfirmEmailDto>
{
    public UserConfirmEmailDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
