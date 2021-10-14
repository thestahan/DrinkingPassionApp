using API.Dtos.Cocktails;
using API.Dtos.Ingredients;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        private readonly IConfiguration _config;
        private readonly IBlobService _blobService;

        public CocktailsController(IMapper mapper, IGenericRepository<Cocktail> cocktailsRepo, IGenericRepository<Ingredient> ingredientsRepo, IConfiguration config, IBlobService blobService)
        {
            _mapper = mapper;
            _cocktailsRepo = cocktailsRepo;
            _ingredientsRepo = ingredientsRepo;
            _config = config;
            _blobService = blobService;
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
        public async Task<ActionResult<CocktailDetailsToReturnDto>> ManageCocktail([FromForm]CocktailToManageDto dto)
        {
            var cocktailFromDto = _mapper.Map<Cocktail>(dto);

            var ingredients = JsonConvert.DeserializeObject<ICollection<IngredientToAddDto>>(dto.Ingredients);

            cocktailFromDto.Ingredients = _mapper.Map<ICollection<Ingredient>>(ingredients);

            cocktailFromDto.IngredientsCount = cocktailFromDto.Ingredients.Count;

            bool editing = cocktailFromDto.Id != 0;

            if (!editing)
            {
                cocktailFromDto.BaseProductId = GetCocktailBaseProductId(cocktailFromDto);

                await _cocktailsRepo.AddAsync(cocktailFromDto);
            }

            var spec = new CocktailWithIngredientsSpecification(cocktailFromDto.Id);

            var cocktailFromDb = await _cocktailsRepo.GetEntityWithSpec(spec);

            bool newPicture = dto.Picture != null && dto.Picture.Length != 0;

            if (newPicture)
            {
                string container = _config["AzureBlobStorage:PublicCocktailPicturesContainer"];

                // delete previous

                bool pictureExists = !string.IsNullOrEmpty(cocktailFromDb.Picture);

                if (pictureExists)
                {
                    string blobName = cocktailFromDb.Picture.Split("/")[1];

                    await _blobService.DeleteBlobAsync(container, blobName);
                }

                // upload new

                // generate name

                string sufix = Guid.NewGuid().ToString().Substring(0, 4);

                string newBlobName = $"cocktail-picture-{cocktailFromDb.Id}-{sufix}";

                // assign new name

                cocktailFromDto.Picture = $"{container}/{newBlobName}";

                // upload file

                await _blobService.UploadFileStreamBlobAsync(container, dto.Picture.OpenReadStream(), newBlobName);
            }

            if (editing)
            {
                MapEditedCocktailToDbCocktail(cocktailFromDto, cocktailFromDb);

                await _cocktailsRepo.UpdateAsync(cocktailFromDb);

                var editedCocktailToReturn = _mapper.Map<CocktailDetailsToReturnDto>(cocktailFromDb);

                return Ok(editedCocktailToReturn);
            }

            if (newPicture) await _cocktailsRepo.UpdateAsync(cocktailFromDto);

            var cocktailToReturn = _mapper.Map<CocktailDetailsToReturnDto>(cocktailFromDb);

            return CreatedAtAction(nameof(GetCocktailById), new { id = cocktailToReturn.Id }, cocktailToReturn);
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
            cocktailFromDb.Picture = cocktail.Picture;
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