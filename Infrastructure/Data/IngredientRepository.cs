using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly AppDbContext _context;

        public IngredientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ingredient> AddIngredientAsync(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return ingredient;
        }

        public async Task<Ingredient> AddIngredientsAsync(List<Ingredient> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                _context.Ingredients.Add(ingredient);
            }

            await _context.SaveChangesAsync();

            return ingredients[0];
        }

        public async Task<Ingredient> GetIngredientByIdAsync(int id)
        {
            return await _context.Ingredients.FindAsync(id);
        }

        public async Task<IReadOnlyList<Ingredient>> GetIngredientsAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }
    }
}
