using System;
using System.Collections.Generic;

namespace DrinkingPassion.Api.Dtos.CocktailsLists
{
    public class CocktailsListDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CocktailsCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public IReadOnlyList<DrinkingPassion.Api.Dtos.Cocktails.CocktailToReturnDto> Cocktails { get; set; }
    }
}
