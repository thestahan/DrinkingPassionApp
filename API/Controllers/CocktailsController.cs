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
        public async Task<ActionResult<CocktailToReturnDto>> GetCocktailById(int id)
        {
            var cocktail = await _cocktailRepo.GetCocktailByIdAsync(id);

            var cocktailToReturn = _mapper.Map<CocktailToReturnDto>(cocktail);

            return Ok(cocktailToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<Cocktail>> AddCocktail(CocktailToAddDto cocktailToAddDto)
        {
            var cocktail = _mapper.Map<Cocktail>(cocktailToAddDto);

            var createdCocktail =  await _cocktailRepo.AddCocktailAsync(cocktail);

            var createdCocktailWithIngredients = await _cocktailRepo.GetCocktailByIdAsync(createdCocktail.Id);

            var cocktailToReturn = _mapper.Map<CocktailToReturnDto>(createdCocktailWithIngredients);

            return CreatedAtAction(nameof(GetCocktailById), new { id = cocktailToReturn.Id }, cocktailToReturn);
        }
    }
}