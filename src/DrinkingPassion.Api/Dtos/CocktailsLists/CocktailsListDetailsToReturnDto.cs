using DrinkingPassion.Shared.Models;
using DrinkingPassion.Shared.Models.Cocktails;
using System;
using System.Collections.Generic;

namespace DrinkingPassion.Api.Dtos.CocktailsLists;

public class CocktailsListDetailsToReturnDto : IQueryDto
{
    public IReadOnlyList<CocktailToReturnDto> Cocktails { get; set; } = [];
    public int CocktailsCount { get; set; }
    public DateTime CreatedDate { get; set; }
    public required int Id { get; set; }
    public string? Name { get; set; }
}
