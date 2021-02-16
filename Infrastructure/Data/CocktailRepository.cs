using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class CocktailRepository : ICocktailRepository
    {
        private readonly AppDbContext _context;

        public CocktailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cocktail> AddCocktailAsync(Cocktail cocktail)
        {
            _context.Cocktails.Add(cocktail);
            await _context.SaveChangesAsync();

            return cocktail;
        }

        public async Task<Cocktail> GetCocktailByIdAsync(int id)
        {
            return await _context.Cocktails.FindAsync(id);
        }

        public async Task<IReadOnlyList<Cocktail>> GetCocktailsAsync()
        {
            return await _context.Cocktails.ToListAsync();
        }
    }
}
