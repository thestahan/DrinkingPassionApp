using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class CocktailsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Cocktail> _repo;

        public CocktailsController(IMapper mapper, IGenericRepository<Cocktail> repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cocktail>>> GetCocktails()
        {
            var spec = new CocktailsWithIngredientsSpecification();

            var cocktailsFromDb = await _repo.ListAsync(spec);

            var cocktailsToReturn = _mapper.Map<IReadOnlyList<CocktailToReturnDto>>(cocktailsFromDb);

            return Ok(cocktailsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CocktailToReturnDto>> GetCocktailById(int id)
        {
            var spec = new CocktailsWithIngredientsSpecification(id);

            var cocktail = await _repo.GetEntityWithSpec(spec);

            if (cocktail == null) return NotFound(new ApiResponse(404));

            var cocktailToReturn = _mapper.Map<CocktailToReturnDto>(cocktail);

            return Ok(cocktailToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<Cocktail>> AddCocktail(CocktailToAddDto cocktailToAddDto)
        {
            var cocktail = _mapper.Map<Cocktail>(cocktailToAddDto);

            var createdCocktail =  await _repo.AddAsync(cocktail);

            var spec = new CocktailsWithIngredientsSpecification(createdCocktail.Id);

            var createdCocktailWithIngredients = await _repo.GetEntityWithSpec(spec);

            var cocktailToReturn = _mapper.Map<CocktailToReturnDto>(createdCocktailWithIngredients);

            return CreatedAtAction(nameof(GetCocktailById), new { id = cocktailToReturn.Id }, cocktailToReturn);
        }
    }
}