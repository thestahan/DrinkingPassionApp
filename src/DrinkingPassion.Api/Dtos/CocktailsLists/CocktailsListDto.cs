using System;

namespace DrinkingPassion.Api.Dtos.CocktailsLists
{
    public class CocktailsListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int CocktailsCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
