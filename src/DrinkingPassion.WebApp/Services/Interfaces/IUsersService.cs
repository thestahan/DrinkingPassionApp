using DrinkingPassion.Shared.Models.Users;
using DrinkingPassion.WebApp.Features.Auth;
using DrinkingPassion.WebApp.Shared;
using OneOf;

namespace DrinkingPassion.WebApp.Services.Interfaces;

public interface IUsersService
{
    Task<User?> GetUserFromLocalStorage();

    Task<OneOf<User, ApiErrorResponse>> LoginUser(UserLoginDto loginDto);

    Task Logout();
}
