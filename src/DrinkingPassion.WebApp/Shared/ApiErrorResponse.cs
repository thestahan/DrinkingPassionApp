namespace DrinkingPassion.WebApp.Shared;

public record ApiErrorResponse
{
    public string? Message { get; init; }
    public int StatusCode { get; init; }
}