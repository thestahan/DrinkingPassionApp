using System;

namespace DrinkingPassion.Api.Dtos.CocktailsLists
{
    public class CocktailsListDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required int CocktailsCount { get; set; }
        public required DateTime CreatedDate { get; set; }
    }
}
