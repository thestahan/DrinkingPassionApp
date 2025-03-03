namespace DrinkingPassion.WebApp.Features.Login.Dtos;

public record LoginReturnDto
{
    public string DisplayName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public ICollection<string> Roles { get; init; } =
        new List<string>();
    public string Token { get; init; } = default!;
    public string TokenExpiration { get; init; } = default!;
}