using API.Dtos.Cocktails;
using API.Dtos.Ingredients;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CocktailDetailsToReturnDto>> ManageCocktail(CocktailToManageDto dto)
        {
            var cocktail = _mapper.Map<Cocktail>(dto);

            bool editing = cocktail.Id != 0;

            if (!editing)
            {
                cocktail.BaseProductId = GetCocktailBaseProductId(cocktail);

                await _cocktailsRepo.AddAsync(cocktail);
            }

            var spec = new CocktailWithIngredientsSpecification(cocktail.Id);

            var cocktailFromDb = await _cocktailsRepo.GetEntityWithSpec(spec);

            if (editing)
            {
                MapEditedCocktailToDbCocktail(cocktail, cocktailFromDb);

                await _cocktailsRepo.UpdateAsync(cocktailFromDb);

                var editedCocktailToReturn = _mapper.Map<CocktailDetailsToReturnDto>(cocktailFromDb);

                return Ok(editedCocktailToReturn);
            }

            var cocktailToReturn = _mapper.Map<CocktailDetailsToReturnDto>(cocktailFromDb);

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

            await UpdateCocktailIngredientsInfo(cocktailId);

            var ingredientToReturn = _mapper.Map<IngredientToReturnDto>(createdIngredient);

            return CreatedAtAction(nameof(GetCocktailById), new { id = cocktailId }, ingredientToReturn);
        }

        [HttpPut("{cocktailId}/ingredients/{id}")]
        public async Task<ActionResult> UpdateCocktailIngredient(int cocktailId, int id, IngredientToUpdateDto ingredient)
        {
            if (!await _cocktailsRepo.EntityExistsAsync(cocktailId)) return BadRequest(new ApiResponse(404, "Cocktail does not exist"));

            var spec = new CocktailIngredientExistsSpecification(cocktailId, id);

            if (!await _ingredientsRepo.EntityExistsWithSpecAsync(spec)) return BadRequest(new ApiResponse(400, "Ingredient was not found in given cocktail"));

            var ingredientToUpdate = new Ingredient
            {
                Id = id,
                Amount = ingredient.Amount
            };

            await _ingredientsRepo.UpdateSpecifiedPropertiesAsync(ingredientToUpdate, nameof(ingredient.Amount));

            await UpdateCocktailBaseProductId(cocktailId);

            return NoContent();
        }

        [HttpDelete("{cocktailId}/ingredients/{id}")]
        public async Task<ActionResult> DeleteCocktailIngredient(int cocktailId, int id)
        {
            if (!await _ingredientsRepo.DeleteByIdAsync(id)) return NotFound(new ApiResponse(404));

            await UpdateCocktailIngredientsInfo(cocktailId);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCocktail(int id)
        {

            var cocktail = await _cocktailsRepo.GetByIdAsync(id);

            if (cocktail == null) return NotFound(new ApiResponse(404));

            await _cocktailsRepo.DeleteAsync(cocktail);

            return NoContent();
        }

        private async Task UpdateCocktailBaseProductId(int cocktailId)
        {
            int baseProductId = await GetBaseProductId(cocktailId);

            var cocktailToUpdate = new Cocktail
            {
                Id = cocktailId,
                BaseProductId = baseProductId
            };

            await _cocktailsRepo.UpdateSpecifiedPropertiesAsync(cocktailToUpdate, nameof(cocktailToUpdate.BaseProductId));
        }

        private async Task UpdateCocktailIngredientsInfo(int cocktailId)
        {
            int ingredientsCount = await GetCocktailIngredientsCount(cocktailId);

            int baseProductId = await GetBaseProductId(cocktailId);

            var cocktailToUpdate = new Cocktail 
            { 
                Id = cocktailId, 
                IngredientsCount = ingredientsCount, 
                BaseProductId = baseProductId
            };

            await _cocktailsRepo.UpdateSpecifiedPropertiesAsync(cocktailToUpdate, nameof(cocktailToUpdate.IngredientsCount), nameof(cocktailToUpdate.BaseProductId));
        }

        private async Task<int> GetBaseProductId(int cocktailId)
        {
            var spec = new IngredientProductIdByBiggestAmount(cocktailId);

            return Convert.ToInt32(await _ingredientsRepo.GetSpecifiedEntityFieldsWithSpecAsync(spec));
        }

        private async Task<int> GetCocktailIngredientsCount(int cocktailId)
        {
            var spec = new IngredientsCountForCocktailSpecification(cocktailId);

            var ingredientsCount = await _ingredientsRepo.CountAsync(spec);
            return ingredientsCount;
        }

        private static void MapEditedCocktailToDbCocktail(Cocktail cocktail, Cocktail cocktailFromDb)
        {
            cocktailFromDb.BaseProductId = GetCocktailBaseProductId(cocktail);
            cocktailFromDb.Description = cocktail.Description;
            cocktailFromDb.Name = cocktail.Name;
            cocktailFromDb.PreparationInstruction = cocktail.PreparationInstruction;
            cocktailFromDb.Ingredients = cocktail.Ingredients;
            cocktailFromDb.IngredientsCount = cocktail.Ingredients.Count;
        }

        private static int GetCocktailBaseProductId(Cocktail cocktail) =>
            cocktail.Ingredients.OrderByDescending(x => x.Amount)
                .Select(x => x.ProductId)
                .FirstOrDefault();
    }
}