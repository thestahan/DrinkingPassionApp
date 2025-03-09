﻿using DrinkingPassion.Api.Core.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace DrinkingPassion.Api.Dtos.Accounts;

public class UserUpdateDto : ICommandDto
{
    [Required]
    public required BartenderType BartenderType { get; set; }

    [Required]
    [StringLength(40, MinimumLength = 3)]
    public required string DisplayName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(60, MinimumLength = 4)]
    public required string Email { get; set; }

    [StringLength(60, MinimumLength = 2)]
    public string? FirstName { get; set; }

    [StringLength(60, MinimumLength = 2)]
    public string? LastName { get; set; }
}
