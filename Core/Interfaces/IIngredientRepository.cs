using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IIngredientRepository
    {
        Task<Ingredient> GetIngredientByIdAsync(int id);
        Task<IReadOnlyList<Ingredient>> GetIngredientsAsync();
        Task<Ingredient> AddIngredientAsync(Ingredient ingredient);
        Task<Ingredient> AddIngredientsAsync(List<Ingredient> ingredient);
    }
}
