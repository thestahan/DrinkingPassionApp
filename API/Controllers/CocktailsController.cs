using API.Dtos.Cocktails;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<Pagination<CocktailToReturnDto>>> GetCocktails([FromQuery]CocktailSpecParams cocktailParams)
        {
            var spec = new CocktailsWithIngredientsCountSpecification(cocktailParams);

            var countSpec = new CocktailsWithFiltersForCountSpecification(cocktailParams);

            var totalItems = await _repo.CountAsync(countSpec);

            var cocktailsFromDb = await _repo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<CocktailToReturnDto>>(cocktailsFromDb);

            return Ok(new Pagination<CocktailToReturnDto>(cocktailParams.PageIndex,
                cocktailParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CocktailDetailsToReturnDto>> GetCocktailById(int id)
        {
            var spec = new CocktailWithIngredientsSpecification(id);

            var cocktail = await _repo.GetEntityWithSpec(spec);

            if (cocktail == null) return NotFound(new ApiResponse(404));

            var cocktailToReturn = _mapper.Map<CocktailDetailsToReturnDto>(cocktail);

            return Ok(cocktailToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<CocktailDetailsToReturnDto>> AddCocktail(CocktailToAddDto cocktailToAddDto)
        {
            var cocktail = _mapper.Map<Cocktail>(cocktailToAddDto);

            var createdCocktail =  await _repo.AddAsync(cocktail);

            var spec = new CocktailWithIngredientsSpecification(createdCocktail.Id);

            var createdCocktailWithIngredients = await _repo.GetEntityWithSpec(spec);

            var cocktailToReturn = _mapper.Map<CocktailDetailsToReturnDto>(createdCocktailWithIngredients);

            return CreatedAtAction(nameof(GetCocktailById), new { id = cocktailToReturn.Id }, cocktailToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCocktailInfo(int id, CocktailInfoToUpdateDto cocktailToUpdate)
        {
            if (id != cocktailToUpdate.Id) return BadRequest(new ApiResponse(400, "Id does not match with entity's id"));

            if (!await _repo.EntityExists(id)) return BadRequest(new ApiResponse(400, "Cocktail does not exist"));

            var cocktailFromDb = await _repo.GetByIdAsync(id);

            var cocktail = CocktailUpdateHelpers.ApplyCocktailInfoChangesToCocktail(cocktailToUpdate, cocktailFromDb);

            await _repo.UpdateAsync(cocktail);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var spec = new CocktailWithIngredientsOnlySpecification(id);

            var cocktail = await _repo.GetEntityWithSpec(spec);

            if (cocktail == null) return NotFound(new ApiResponse(404));

            await _repo.DeleteAsync(cocktail);

            return NoContent();
        }
    }
}