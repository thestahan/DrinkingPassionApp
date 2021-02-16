using API.Dtos;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CocktailsController(ICocktailRepository cocktailRepo, IMapper mapper)
        {
            _cocktailRepo = cocktailRepo;
            _mapper = mapper;
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
            var cocktail = _mapper.Map<Cocktail>(cocktailDto);

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
            var cocktailToReturn = _cocktailRepo.GetCocktailByIdAsync(createdCocktail.Id);

            return CreatedAtAction(nameof(GetCocktailById), new { id = cocktailToReturn.Id }, cocktailToReturn);
        }
    }
}
