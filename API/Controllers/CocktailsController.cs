using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CocktailsController : ControllerBase
    {
        private readonly ICocktailRepository _cocktailRepo;

        public CocktailsController(ICocktailRepository cocktailRepo)
        {
            _cocktailRepo = cocktailRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cocktail>>> GetCocktails()
        {
            var cocktails = await _cocktailRepo.GetCocktailsAsync();

            return Ok(cocktails);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cocktail>> GetCocktailById(int id)
        {
            var cocktail = await _cocktailRepo.GetCocktailByIdAsync(id);

            return Ok(cocktail);
        }

        [HttpPost]
        public async Task<ActionResult<Cocktail>> AddCocktail(CocktailToAddDto cocktailDto)
        {
            var cocktail = new Cocktail
            {
                Name = cocktailDto.Name,
                Description = cocktailDto.Description,
                Picture = cocktailDto.Picture,
                PreparationInstruction = cocktailDto.PreparationInstruction
            };

            var ingredients = new List<Ingredient>();

            foreach (var ingredient in cocktailDto.Ingredients)
            {
                ingredients.Add(new Ingredient
                {
                    Amount = ingredient.Amount,
                    ProductId = ingredient.ProductId,
                    Cocktail = cocktail
                });
            }

            cocktail.Ingredients = ingredients;

            var createdCocktail =  await _cocktailRepo.AddCocktailAsync(cocktail);

            return Ok();
        }
    }
}
