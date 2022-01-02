using System;

namespace API.Dtos.CocktailsLists
{
    public class CocktailsListDto
    {
        public int Id { get; set; }
        public Guid UniqueLink { get; set; }
        public string Name { get; set; }
        public int CocktailsCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
