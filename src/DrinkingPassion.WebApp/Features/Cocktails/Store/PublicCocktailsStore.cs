using DrinkingPassion.Shared.Models;
using DrinkingPassion.Shared.Models.Cocktails;
using DrinkingPassion.WebApp.Services.Interfaces;
using Fluxor;

namespace DrinkingPassion.WebApp.Features.Cocktails.Store;

public record PublicCocktailsState
{
    public string ErrorMessage { get; init; } = string.Empty;
    public bool IsError { get; init; }
    public bool IsInitialized { get; init; }
    public bool IsLoading { get; init; }
    public Pagination<CocktailToReturnDto>? PaginatedCocktails { get; init; }
}

public class PublicCocktailsFeature : Feature<PublicCocktailsState>
{
    protected override PublicCocktailsState GetInitialState()
    {
        return new()
        {
            IsInitialized = false,
            IsLoading = false,
            IsError = false,
            PaginatedCocktails = null
        };
    }

    public override string GetName()
    {
        return nameof(PublicCocktailsFeature);
    }
}

public static class PublicCocktailsReducers
{
    [ReducerMethod]
    public static PublicCocktailsState OnFetchPublicCocktails(PublicCocktailsState state, FetchPublicCocktailsAction _)
    {
        return state with
        {
            IsLoading = true
        };
    }

    [ReducerMethod]
    public static PublicCocktailsState OnFetchPublicCocktailsFailure(PublicCocktailsState state, FetchPublicCocktailsFailureAction action)
    {
        return state with
        {
            IsInitialized = true,
            IsLoading = false,
            IsError = true,
            ErrorMessage = action.ErrorMessage
        };
    }

    [ReducerMethod]
    public static PublicCocktailsState OnFetchPublicCocktailsSuccess(PublicCocktailsState state, FetchPublicCocktailsSuccessAction action)
    {
        return state with
        {
            IsInitialized = true,
            IsLoading = false,
            PaginatedCocktails = action.PaginatedCocktails
        };
    }
}

public class PublicCocktailsEffects(ICocktailsService cocktailsService)
{
    [EffectMethod]
    public async Task HandleFetchPublicCocktailsAction(FetchPublicCocktailsAction action, IDispatcher dispatcher)
    {
        try
        {
            var paginatedCocktails = await cocktailsService.GetPublicCocktails(action.PageIndex);

            dispatcher.Dispatch(new FetchPublicCocktailsSuccessAction(paginatedCocktails!));
        }
        catch (Exception e)
        {
            dispatcher.Dispatch(new FetchPublicCocktailsFailureAction(e.Message));
        }
    }
}

public record FetchPublicCocktailsAction(int PageIndex);
public record FetchPublicCocktailsSuccessAction(Pagination<CocktailToReturnDto> PaginatedCocktails);
public record FetchPublicCocktailsFailureAction(string ErrorMessage);
