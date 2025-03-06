using DrinkingPassion.Api.Dtos.Cocktails;
using System;
using System.Collections.Generic;

namespace DrinkingPassion.Api.Dtos.CocktailsLists
{
    public class CocktailsListDetailsDto
    {
        public required int Id { get; set; }
        public string? Name { get; set; }
        public int CocktailsCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public IReadOnlyList<CocktailToReturnDto> Cocktails { get; set; } = [];
    }
}
