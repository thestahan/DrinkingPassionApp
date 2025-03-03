using Fluxor;

namespace DrinkingPassion.WebApp.Features.Auth;

public record UserState
{
    public string? DisplayName { get; init; }
    public string? Email { get; init; }
    public bool IsUserSet { get; set; }
    public ICollection<string> Roles { get; init; } = new List<string>();
}

public class UserFeature : Feature<UserState>
{

    protected override UserState GetInitialState()
    {
        return new()
        {
            Email = null,
            DisplayName = null,
            Roles = new List<string>(),
            IsUserSet = false
        };
    }

    public override string GetName()
    {
        return nameof(UserFeature);
    }
}

public static class UserReducers
{
    [ReducerMethod]
    public static UserState OnUserLoggedIn(UserState state, UserLoggedInAction action)
    {
        return state with
        {
            Email = action.Email,
            DisplayName = action.DisplayName,
            Roles = action.Roles,
            IsUserSet = true
        };
    }

    [ReducerMethod]
    public static UserState OnUserLogout(UserState state, UserLogoutAction _)
    {
        return state with
        {
            Email = null,
            DisplayName = null,
            Roles = new List<string>(),
            IsUserSet = false
        };
    }
}

public class UserEffects
{
    private readonly DrinkingPassionAuthenticationStateProvider _authenticationStateProvider;

    public UserEffects(DrinkingPassionAuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    [EffectMethod(typeof(UserLogoutAction))]
    public async Task HandleUserLogoutAction(IDispatcher _)
    {
        await _authenticationStateProvider.Logout();
    }
}

public record UserLoggedInAction(string Email, string? DisplayName, ICollection<string> Roles);
public record UserLogoutAction();