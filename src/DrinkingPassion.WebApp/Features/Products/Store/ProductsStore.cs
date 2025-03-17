using DrinkingPassion.Shared.Models.Products;
using Fluxor;

namespace DrinkingPassion.WebApp.Features.Products.Store;

public record PublicProductsState
{
    public string ErrorMessage { get; init; } = string.Empty;
    public bool IsError { get; init; }
    public bool IsInitialized { get; init; }
    public bool IsLoading { get; init; }
    public IReadOnlyList<ProductToReturnDto> Products { get; init; } = [];
}

public class PublicProductsFeature : Feature<PublicProductsState>
{
    protected override PublicProductsState GetInitialState()
    {
        return new()
        {
            IsInitialized = false,
            IsLoading = false,
            IsError = false,
            Products = []
        };
    }

    public override string GetName()
    {
        return nameof(PublicProductsFeature);
    }
}

public static class ProductsReducers
{
    [ReducerMethod]
    public static PublicProductsState OnFetchProducts(PublicProductsState state, FetchPublicProductsAction _)
    {
        return state with
        {
            IsLoading = true
        };
    }

    [ReducerMethod]
    public static PublicProductsState OnFetchProductsFailure(PublicProductsState state, FetchPublicProductsFailureAction action)
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
    public static PublicProductsState OnFetchProductsSuccess(PublicProductsState state, FetchPublicProductsSuccessAction action)
    {
        return state with
        {
            IsInitialized = true,
            IsLoading = false,
            IsError = false,
            Products = action.Products
        };
    }
}

public class ProductsEffects(Services.Interfaces.IProductsService productsService)
{
    [EffectMethod]
    public async Task HandleFetchProductsAction(FetchPublicProductsAction action, IDispatcher dispatcher)
    {
        try
        {
            var products = await productsService.GetProducts();
            dispatcher.Dispatch(new FetchPublicProductsSuccessAction(products));
        }
        catch (Exception e)
        {
            dispatcher.Dispatch(new FetchPublicProductsFailureAction(e.Message));
        }
    }
}

public record class FetchPublicProductsAction();
public record class FetchPublicProductsSuccessAction(IReadOnlyList<ProductToReturnDto> Products);
public record class FetchPublicProductsFailureAction(string ErrorMessage);
