using API.Dtos.Cocktails;
using API.Dtos.Ingredients;
using API.Errors;
using API.Helpers;
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
        private readonly IGenericRepository<Cocktail> _cocktailsRepo;
        private readonly IGenericRepository<Ingredient> _ingredientsRepo;

        public CocktailsController(IMapper mapper, IGenericRepository<Cocktail> cocktailsRepo, IGenericRepository<Ingredient> ingredientsRepo)
        {
            _mapper = mapper;
            _cocktailsRepo = cocktailsRepo;
            _ingredientsRepo = ingredientsRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<CocktailToReturnDto>>> GetCocktails([FromQuery]CocktailSpecParams cocktailParams)
        {
            var spec = new CocktailsWithIngredientsCountSpecification(cocktailParams);

            var countSpec = new CocktailsWithFiltersForCountSpecification(cocktailParams);

            var totalItems = await _cocktailsRepo.CountAsync(countSpec);

            var cocktailsFromDb = await _cocktailsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<CocktailToReturnDto>>(cocktailsFromDb);

            return Ok(new Pagination<CocktailToReturnDto>(cocktailParams.PageIndex,
                cocktailParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CocktailDetailsToReturnDto>> GetCocktailById(int id)
        {
            var spec = new CocktailWithIngredientsSpecification(id);

            var cocktail = await _cocktailsRepo.GetEntityWithSpec(spec);

            if (cocktail == null) return NotFound(new ApiResponse(404));

            var cocktailToReturn = _mapper.Map<CocktailDetailsToReturnDto>(cocktail);

            return Ok(cocktailToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<CocktailDetailsToReturnDto>> AddCocktail(CocktailToAddDto cocktailToAddDto)
        {
            var cocktail = _mapper.Map<Cocktail>(cocktailToAddDto);

            var createdCocktail =  await _cocktailsRepo.AddAsync(cocktail);

            var spec = new CocktailWithIngredientsSpecification(createdCocktail.Id);

            var createdCocktailWithIngredients = await _cocktailsRepo.GetEntityWithSpec(spec);

            var cocktailToReturn = _mapper.Map<CocktailDetailsToReturnDto>(createdCocktailWithIngredients);

            return CreatedAtAction(nameof(GetCocktailById), new { id = cocktailToReturn.Id }, cocktailToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCocktailInfo(int id, CocktailInfoToUpdateDto cocktailToUpdate)
        {
            if (!await _cocktailsRepo.EntityExistsAsync(id)) return BadRequest(new ApiResponse(404, "Cocktail does not exist"));

            if (id != cocktailToUpdate.Id) return BadRequest(new ApiResponse(400, "Id does not match with entity's id"));

            var cocktailFromDb = await _cocktailsRepo.GetByIdAsync(id);

            var cocktail = CocktailUpdateHelpers.ApplyCocktailInfoChangesToCocktail(cocktailToUpdate, cocktailFromDb);

            await _cocktailsRepo.UpdateAsync(cocktail);

            return NoContent();
        }

        [HttpPost("{cocktailId}/ingredients")]
        public async Task<ActionResult<IngredientToReturnDto>> AddIngredientToCocktail(int cocktailId, IngredientToAddDto ingredient)
        {
            var ingredientToAdd = _mapper.Map<Ingredient>(ingredient);

            ingredientToAdd.CocktailId = cocktailId;

            var createdIngredient = await _ingredientsRepo.AddAsync(ingredientToAdd);

            await UpdateCocktailIngredientsCount(cocktailId);

            var ingredientToReturn = _mapper.Map<IngredientToReturnDto>(createdIngredient);

            return CreatedAtAction(nameof(GetCocktailById), new { id = cocktailId }, ingredientToReturn);
        }

        [HttpPut("{cocktailId}/ingredients/{id}")]
        public async Task<ActionResult> UpdateCocktailIngredient(int cocktailId, int id, IngredientToUpdateDto ingredient)
        {
            if (!await _cocktailsRepo.EntityExistsAsync(cocktailId)) return BadRequest(new ApiResponse(404, "Cocktail does not exist"));

            var spec = new CocktailIngredientExistsSpecification(cocktailId, id);

            if (!await _ingredientsRepo.EntityExistsWithSpecAsync(spec)) return BadRequest(new ApiResponse(400, "Ingredient was not found in given cocktail"));

            var ingredientToUpdate = _mapper.Map<Ingredient>(ingredient);

            ingredientToUpdate.CocktailId = cocktailId;

            await _ingredientsRepo.UpdateAsync(ingredientToUpdate);

            return NoContent();
        }

        [HttpDelete("{cocktailId}/ingredients/{id}")]
        public async Task<ActionResult> DeleteCocktailIngredient(int cocktailId, int id)
        {
            if (!await _ingredientsRepo.DeleteByIdAsync(id)) return NotFound(new ApiResponse(404));

            await UpdateCocktailIngredientsCount(cocktailId);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var spec = new CocktailWithIngredientsOnlySpecification(id);

            var cocktail = await _cocktailsRepo.GetEntityWithSpec(spec);

            if (cocktail == null) return NotFound(new ApiResponse(404));

            await _cocktailsRepo.DeleteAsync(cocktail);

            return NoContent();
        }

        private async Task UpdateCocktailIngredientsCount(int cocktailId)
        {
            var spec = new IngredientsCountForCocktailSpecification(cocktailId);

            var ingredientsCount = await _ingredientsRepo.CountAsync(spec);

            var cocktailToUpdate = new Cocktail { Id = cocktailId, IngredientsCount = ingredientsCount };

            await _cocktailsRepo.UpdateSpecifiedPropertiesAsync(cocktailToUpdate, nameof(cocktailToUpdate.IngredientsCount));
        }
    }
}