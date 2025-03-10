using DrinkingPassion.Api.Core.Entities.Enums;
using DrinkingPassion.Shared.Models;
using FluentValidation;

namespace DrinkingPassion.Api.Dtos.Accounts;

public class UserRegisterDto : ICommandDto
{
    public required BartenderType BartenderType { get; set; }
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Password { get; set; }
}

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(x => x.BartenderType).IsInEnum();
        RuleFor(x => x.DisplayName).NotEmpty().Length(3, 40);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().Length(4, 60);
        RuleFor(x => x.FirstName).Length(2, 60);
        RuleFor(x => x.LastName).Length(2, 60);
        RuleFor(x => x.Password).NotEmpty();
    }
}
