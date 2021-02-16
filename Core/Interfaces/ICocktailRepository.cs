using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICocktailRepository
    {
        Task<Cocktail> GetCocktailByIdAsync(int id);
        Task<IReadOnlyList<Cocktail>> GetCocktailsAsync();
        Task<Cocktail> AddCocktailAsync(Cocktail cocktail);
    }
}