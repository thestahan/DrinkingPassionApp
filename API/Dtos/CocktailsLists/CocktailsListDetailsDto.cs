using API.Dtos.Cocktails;
using System;
using System.Collections.Generic;

namespace API.Dtos.CocktailsLists
{
    public class CocktailsListDetailsDto
    {
        public Guid UniqueLink { get; set; }
        public string Name { get; set; }
        public int CocktailsCount { get; set; }
        public IReadOnlyList<CocktailToReturnDto> Cocktails { get; set; }
    }
}
