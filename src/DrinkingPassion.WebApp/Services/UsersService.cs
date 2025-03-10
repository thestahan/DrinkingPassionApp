using Blazored.LocalStorage;
using DrinkingPassion.Shared.Models.Users;
using Newtonsoft.Json;
using OneOf;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DrinkingPassion.WebApp.Services;

public class UsersService : Interfaces.IUsersService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public UsersService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    private static ClaimsPrincipal CreateClaimsPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var identity = new ClaimsIdentity();

        if (tokenHandler.CanReadToken(token))
        {
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            identity = new(jwtSecurityToken.Claims, Features.Auth.AuthConstants.AuthenticationType);
        }

        return new(identity);
    }

    public async Task<Features.Auth.User?> GetUserFromLocalStorage()
    {
        var token = await _localStorage.GetItemAsync<string?>("Token");

        if (token is null)
        {
            return null;
        }

        var claimsPrincipal = CreateClaimsPrincipalFromToken(token);

        return Features.Auth.User.FromClaimsPrincipal(claimsPrincipal);
    }

    public async Task<OneOf<Features.Auth.User, Shared.ApiErrorResponse>> LoginUser(UserLoginDto loginDto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/accounts/login")
        {
            Content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json")
        };

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<Shared.ApiErrorResponse>(responseContent)!;
        }

        var returnDto = JsonConvert.DeserializeObject<UserLoginReturnDto>(responseContent)!;
        var claimsPrincipal = CreateClaimsPrincipalFromToken(returnDto.Token);
        var user = Features.Auth.User.FromClaimsPrincipal(claimsPrincipal);

        await _localStorage.SetItemAsync("Token", returnDto.Token);
        await _localStorage.SetItemAsync("TokenExpiration", returnDto.TokenExpiration);

        return user;
    }

    public async Task Logout()
    {
        await _localStorage.ClearAsync();
    }
}
