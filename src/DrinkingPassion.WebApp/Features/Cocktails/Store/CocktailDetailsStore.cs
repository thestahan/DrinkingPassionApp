using Fluxor;

namespace DrinkingPassion.WebApp.Features.Cocktails.Store;

public record CocktailDetailsState
{
    public Dtos.CocktailDetails? CocktailDetails { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public bool IsError { get; init; }
    public bool IsInitialized { get; set; }
    public bool IsLoading { get; init; }
}

public class CocktailDetailsFeature : Feature<CocktailDetailsState>
{

    protected override CocktailDetailsState GetInitialState()
    {
        return new()
        {
            IsInitialized = false,
            IsLoading = false,
            IsError = false,
            CocktailDetails = null
        };
    }

    public override string GetName()
    {
        return nameof(CocktailDetailsFeature);
    }
}

public static class CocktailDetailsReducers
{
    [ReducerMethod]
    public static CocktailDetailsState OnFetchCocktailDetails(CocktailDetailsState state, FetchCocktailDetailsAction _)
    {
        return state with
        {
            IsLoading = true
        };
    }

    [ReducerMethod]
    public static CocktailDetailsState OnFetchCocktailDetailsFailure(CocktailDetailsState state, FetchCocktailDetailsFailureAction action)
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
    public static CocktailDetailsState OnFetchCocktailDetailsSuccess(CocktailDetailsState state, FetchCocktailDetailsSuccessAction action)
    {
        return state with
        {
            IsInitialized = true,
            IsLoading = false,
            CocktailDetails = action.CocktailDetails
        };
    }
}

public class CocktailDetailsEffects
{
    private readonly Services.Interfaces.ICocktailsService _cocktailsService;

    public CocktailDetailsEffects(Services.Interfaces.ICocktailsService cocktailsService)
    {
        _cocktailsService = cocktailsService;
    }

    [EffectMethod]
    public async Task HandleFetchCocktailDetailsAction(FetchCocktailDetailsAction action, IDispatcher dispatcher)
    {
        try
        {
            var cocktailDetails = await _cocktailsService.GetCocktailDetails(action.CocktailId);

            dispatcher.Dispatch(new FetchCocktailDetailsSuccessAction(cocktailDetails!));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new FetchCocktailDetailsFailureAction(ex.Message));
        }
    }
}

public record FetchCocktailDetailsAction(int CocktailId);
public record FetchCocktailDetailsSuccessAction(Dtos.CocktailDetails CocktailDetails);
public record FetchCocktailDetailsFailureAction(string ErrorMessage);
