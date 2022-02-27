using API.Dtos.Cocktails;
using System;
using System.Collections.Generic;

namespace API.Dtos.CocktailsLists
{
    public class CocktailsListDetailsDto
    {
        public int Id { get; set; }
        public Guid UniqueLink { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public int CocktailsCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public IReadOnlyList<CocktailToReturnDto> Cocktails { get; set; }
    }
}
