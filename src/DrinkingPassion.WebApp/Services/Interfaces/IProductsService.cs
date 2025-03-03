namespace DrinkingPassion.WebApp.Services.Interfaces;

public interface IProductsService
{

    Task CreateProduct(Features.Products.Dtos.ProductToAddDto product);

    Task DeleteProductAsync(int id);

    Task<Features.Products.Dtos.ProductDto> GetProduct(int id);
    Task<ICollection<Features.Products.Dtos.ProductDto>> GetProducts();

    Task UpdateProduct(int id, Features.Products.Dtos.ProductToUpdateDto product);
}
