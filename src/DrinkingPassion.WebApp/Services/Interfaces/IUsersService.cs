using OneOf;

namespace DrinkingPassion.WebApp.Services.Interfaces;

public interface IUsersService
{

    Task<Features.Auth.User?> GetUserFromLocalStorage();
    Task<OneOf<Features.Auth.User, Shared.ApiErrorResponse>> LoginUser(Features.Login.Dtos.LoginDto loginDto);

    Task Logout();
}