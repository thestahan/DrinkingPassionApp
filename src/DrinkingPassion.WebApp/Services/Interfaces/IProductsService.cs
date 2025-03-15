using DrinkingPassion.Shared.Models.Products;

namespace DrinkingPassion.WebApp.Services.Interfaces;

public interface IProductsService
{
    Task CreateProduct(ProductToAddDto product);

    Task DeleteProductAsync(int id);

    Task<ProductToReturnDto> GetProduct(int id);

    Task<IReadOnlyList<ProductToReturnDto>> GetProducts();

    Task UpdateProduct(int id, ProductToUpdateDto product);
}
