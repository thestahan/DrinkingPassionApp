using DrinkingPassion.Shared.Models;
using System;

namespace DrinkingPassion.Api.Dtos.CocktailsLists;

public class CocktailsListToReturnDto : IQueryDto
{
    public required int CocktailsCount { get; set; }
    public required DateTime CreatedDate { get; set; }
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
}
