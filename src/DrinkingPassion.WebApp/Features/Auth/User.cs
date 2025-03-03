using System.Security.Claims;

namespace DrinkingPassion.WebApp.Features.Auth;

public class User
{

    public static User FromClaimsPrincipal(ClaimsPrincipal principal)
    {
        return new()
        {
            Email = principal.FindFirst("email")?.Value ?? string.Empty,
            DisplayName = principal.FindFirst("given_name")?.Value ?? string.Empty,
            Roles = principal.FindAll("role").Select(c => c.Value).ToList()
        };
    }

    public ClaimsPrincipal ToClaimsPrincipal()
    {
        var rolesClaims = Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray();
        var claims = new Claim[]
        {
            new(ClaimTypes.Email, Email),
            new(ClaimTypes.GivenName, DisplayName ?? string.Empty)
        }.Concat(rolesClaims);

        var claimsIdentity = new ClaimsIdentity(claims, AuthConstants.AuthenticationType);

        return new ClaimsPrincipal(claimsIdentity);
    }

    public string? DisplayName { get; set; }
    public string Email { get; set; } = default!;

    public ICollection<string> Roles { get; set; } =
        new List<string>();
}