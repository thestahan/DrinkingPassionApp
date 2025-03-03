using Fluxor;

namespace DrinkingPassion.WebApp.Features.Products.Store;

public record ProductsState
{
    public string ErrorMessage { get; init; } = string.Empty;
    public bool IsError { get; init; }
    public bool IsInitialized { get; init; }
    public bool IsLoading { get; init; }
    public ICollection<Dtos.ProductDto>? PaginatedProducts { get; init; }
}

public class ProductsFeature : Feature<ProductsState>
{

    protected override ProductsState GetInitialState()
    {
        return new()
        {
            IsInitialized = false,
            IsLoading = false,
            IsError = false,
            PaginatedProducts = null
        };
    }

    public override string GetName()
    {
        return nameof(ProductsFeature);
    }
}

public static class ProductsReducers
{
    [ReducerMethod]
    public static ProductsState OnFetchProducts(ProductsState state, FetchProductsAction _)
    {
        return state with
        {
            IsLoading = true
        };
    }

    [ReducerMethod]
    public static ProductsState OnFetchProductsFailure(ProductsState state, FetchProductsFailureAction action)
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
    public static ProductsState OnFetchProductsSuccess(ProductsState state, FetchProductsSuccessAction action)
    {
        return state with
        {
            IsInitialized = true,
            IsLoading = false,
            IsError = false,
            PaginatedProducts = action.PaginatedProducts
        };
    }
}

public class ProductsEffects
{
    private readonly Services.Interfaces.IProductsService _productsService;

    public ProductsEffects(Services.Interfaces.IProductsService productsService)
    {
        _productsService = productsService;
    }

    [EffectMethod]
    public async Task HandleFetchProductsAction(FetchProductsAction action, IDispatcher dispatcher)
    {
        try
        {
            var products = await _productsService.GetProducts();
            dispatcher.Dispatch(new FetchProductsSuccessAction(products));
        }
        catch (Exception e)
        {
            dispatcher.Dispatch(new FetchProductsFailureAction(e.Message));
        }
    }
}

public record class FetchProductsAction();
public record class FetchProductsSuccessAction(ICollection<Dtos.ProductDto> PaginatedProducts);
public record class FetchProductsFailureAction(string ErrorMessage);
