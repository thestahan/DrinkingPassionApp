using DrinkingPassion.Shared.Models.Users;
using DrinkingPassion.WebApp.Shared;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace DrinkingPassion.WebApp.Features.Login.Store;

public record LoginState
{
    public string? ErrorMessage { get; init; }
    public bool IsError { get; init; }
    public bool IsSending { get; init; }
    public UserLoginDto LoginDto { get; set; } = default!;
}

public class LoginFeature : Feature<LoginState>
{
    protected override LoginState GetInitialState()
    {
        return new()
        {
            IsSending = false,
            IsError = false,
            ErrorMessage = null,
            LoginDto = new UserLoginDto { Email = string.Empty, Password = string.Empty },
        };
    }

    public override string GetName()
    {
        return nameof(LoginFeature);
    }
}

public static class LoginReducers
{
    [ReducerMethod]
    public static LoginState OnLoginFailure(LoginState state, LoginFailureAction action)
    {
        return state with
        {
            IsSending = false,
            IsError = true,
            ErrorMessage = action.ApiErrorResponse.Message
        };
    }

    [ReducerMethod]
    public static LoginState OnLoginSubmit(LoginState state, LoginSubmitAction _)
    {
        return state with
        {
            IsSending = true
        };
    }

    [ReducerMethod]
    public static LoginState OnLoginSuccess(LoginState state, LoginSuccessAction _)
    {
        return state with
        {
            IsSending = false,
            IsError = false
        };
    }
}

public class LoginEffects
{
    private readonly Auth.DrinkingPassionAuthenticationStateProvider _authenticationStateProvider;
    private readonly NavigationManager _navigationManager;

    public LoginEffects(
        Auth.DrinkingPassionAuthenticationStateProvider authenticationStateProvider,
        NavigationManager navigationManager)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _navigationManager = navigationManager;
    }

    [EffectMethod]
    public async Task HandleLoginSubmitAction(LoginSubmitAction action, IDispatcher dispatcher)
    {
        var result = await _authenticationStateProvider.Login(action.LoginDto);

        result.Switch(
            user =>
            {
                dispatcher.Dispatch(new LoginSuccessAction(user));
                dispatcher.Dispatch(new Auth.UserLoggedInAction(
                    Email: user.Email,
                    DisplayName: user.DisplayName,
                    Roles: user.Roles));

                _navigationManager.NavigateTo("/");
            },
            error => dispatcher.Dispatch(new LoginFailureAction(error)));
    }
}

public record LoginSuccessAction(Auth.User User);
public record LoginFailureAction(ApiErrorResponse ApiErrorResponse);
public record LoginSubmitAction(UserLoginDto LoginDto);
