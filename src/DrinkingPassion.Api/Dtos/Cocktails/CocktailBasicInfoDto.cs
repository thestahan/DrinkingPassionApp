namespace DrinkingPassion.Api.Dtos.Cocktails
{
    public class CocktailBasicInfoDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required bool IsPrivate { get; set; }
    }
}
