using DrinkingPassion.Shared.Models.Users;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace DrinkingPassion.WebApp.Features.Login.Pages;

public partial class LoginPage : FluxorComponent
{
    private void SubmitForm()
    {
        Dispatcher.Dispatch(new Store.LoginSubmitAction(Model));
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var user = (await AuthState).User;
        var userIdentity = user.Identity;
        if ((userIdentity is not null) && userIdentity.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/");
        }
    }

    protected MudForm Form = default!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    [Inject]
    public IDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    public IState<Store.LoginState> LoginState { get; set; } = default!;

    public UserLoginDto Model => LoginState.Value.LoginDto;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IState<Auth.UserState> UserState { get; set; } = default!;
}
