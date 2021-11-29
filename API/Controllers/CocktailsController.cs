using API.Dtos.Cocktails;
using API.Dtos.Ingredients;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly CocktailPicturesService _cocktailPicturesService;
        private readonly UserManager<AppUser> _userManager;

        public CocktailsController(
            IMapper mapper,
            IGenericRepository<Cocktail> cocktailsRepo,
            IGenericRepository<Ingredient> ingredientsRepo,
            IConfiguration config,
            IBlobService blobService,
            UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _cocktailsRepo = cocktailsRepo;
            _ingredientsRepo = ingredientsRepo;
            _config = config;
            _blobService = blobService;
            _cocktailPicturesService = new CocktailPicturesService(blobService);
            _userManager = userManager;
        }

        [HttpGet("Public")]
        public async Task<ActionResult<Pagination<CocktailToReturnDto>>> GetCocktails([FromQuery]CocktailSpecParams cocktailParams)
        {
            var spec = new CocktailsWithIngredientsCountSpecification(cocktailParams, false);

            var countSpec = new CocktailsWithFiltersForCountSpecification(cocktailParams, false);

            var totalItems = await _cocktailsRepo.CountAsync(countSpec);

            var cocktailsFromDb = await _cocktailsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<CocktailToReturnDto>>(cocktailsFromDb);

            return Ok(new Pagination<CocktailToReturnDto>(cocktailParams.PageIndex,
                cocktailParams.PageSize, totalItems, data));
        }

        [Authorize]
        [HttpGet("Private")]
        public async Task<ActionResult<Pagination<CocktailToReturnDto>>> GetPrivateCocktails([FromQuery] CocktailSpecParams cocktailParams)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            var spec = new CocktailsWithIngredientsCountSpecification(cocktailParams, true, user.Id);

            var countSpec = new CocktailsWithFiltersForCountSpecification(cocktailParams, true, user.Id);

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

            if (cocktail.IsPrivate)
            {
                var user = await GetAuthorizedUser();

                if (user == null || cocktail.AuthorId != user.Id) return NotFound(new ApiResponse(404));
            }

            if (cocktail == null) return NotFound(new ApiResponse(404));

            var cocktailToReturn = _mapper.Map<CocktailDetailsToReturnDto>(cocktail);

            return Ok(cocktailToReturn);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CocktailDetailsToReturnDto>> ManageCocktail([FromForm]CocktailToManageDto dto)
        {
            var user = await GetAuthorizedUser();

            var cocktailFromDto = _mapper.Map<Cocktail>(dto);

            var ingredients = JsonConvert.DeserializeObject<ICollection<IngredientToAddDto>>(dto.Ingredients);

            cocktailFromDto.Ingredients = _mapper.Map<ICollection<Ingredient>>(ingredients);

            cocktailFromDto.IngredientsCount = cocktailFromDto.Ingredients.Count;

            cocktailFromDto.AuthorId = user.Id;

            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            if (!isAdmin) cocktailFromDto.IsPrivate = true;

            bool editing = cocktailFromDto.Id != 0;

            if (!editing)
            {
                cocktailFromDto.BaseProductId = GetCocktailBaseProductId(cocktailFromDto);

                await _cocktailsRepo.AddAsync(cocktailFromDto);
            }

            var spec = new CocktailByPrivacyWithIngredientsSpecification(cocktailFromDto.Id, dto.IsPrivate);

            var cocktailFromDb = await _cocktailsRepo.GetEntityWithSpec(spec);

            if (cocktailFromDb == null) return NotFound(new ApiResponse(404));

            bool newPicture = dto.Picture != null && dto.Picture.Length != 0;

            if (newPicture)
            {
                string container = _config["AzureBlobStorage:PublicCocktailPicturesContainer"];

                await _cocktailPicturesService.DeletePreviousPictureIfExists(cocktailFromDb, container);

                var newBlobName = _cocktailPicturesService.GenerateNewBlobName(cocktailFromDb.Id);

                cocktailFromDto.Picture = $"{container}/{newBlobName}";

                await _blobService.UploadFileStreamBlobAsync(container, dto.Picture.OpenReadStream(), newBlobName);
            }

            if (editing)
            {
                MapEditedCocktailToDbCocktail(cocktailFromDto, cocktailFromDb, !newPicture);

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
            var user = await GetAuthorizedUser();

            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            var cocktail = await _cocktailsRepo.GetByIdAsync(id);

            if (cocktail == null) return NotFound(new ApiResponse(404));

            if (!cocktail.IsPrivate && !isAdmin) return Unauthorized(new ApiResponse(401));

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

        private static void MapEditedCocktailToDbCocktail(Cocktail cocktail, Cocktail cocktailFromDb, bool skipPicture)
        {
            if (!skipPicture) cocktailFromDb.Picture = cocktail.Picture;
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

        private async Task<Cocktail> GetCocktailFromDbByPrivacy(CocktailToManageDto dto, int cocktailId)
        {
            if (dto.IsPrivate)
            {
                var spec = new CocktailByPrivacyWithIngredientsSpecification(cocktailId, dto.IsPrivate);

                return await _cocktailsRepo.GetEntityWithSpec(spec);
            }
            else
            {
                var spec = new CocktailWithIngredientsSpecification(cocktailId);

                return await _cocktailsRepo.GetEntityWithSpec(spec);
            }
        }

        private async Task<AppUser> GetAuthorizedUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email)) return null;

            return await _userManager.FindByEmailAsync(email);
        }
    }
}