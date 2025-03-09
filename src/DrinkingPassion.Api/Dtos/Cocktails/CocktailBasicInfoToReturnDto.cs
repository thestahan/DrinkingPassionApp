namespace DrinkingPassion.Api.Dtos.Cocktails;

public class CocktailBasicInfoToReturnDto : IQueryDto
{
    public required int Id { get; set; }
    public required bool IsPrivate { get; set; }
    public required string Name { get; set; }
}
