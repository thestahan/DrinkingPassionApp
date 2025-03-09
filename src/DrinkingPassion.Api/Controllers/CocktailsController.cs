using AutoMapper;
using DrinkingPassion.Api.Core.Entities;
using DrinkingPassion.Api.Core.Entities.Identity;
using DrinkingPassion.Api.Core.Interfaces;
using DrinkingPassion.Api.Core.Specifications.Cocktails;
using DrinkingPassion.Api.Dtos.Cocktails;
using DrinkingPassion.Api.Dtos.Ingredients;
using DrinkingPassion.Api.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DrinkingPassion.Api.Controllers
{
    public class CocktailsController : DrinkingPassion.Api.Controllers.BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Cocktail> _cocktailsRepo;
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IConfiguration _config;
        private readonly IBlobService _blobService;
        private readonly CocktailPicturesService _cocktailPicturesService;
        private readonly UserManager<AppUser> _userManager;

        public CocktailsController(
            IMapper mapper,
            IGenericRepository<Cocktail> cocktailsRepo,
            IGenericRepository<Product> productsRepo,
            IConfiguration config,
            IBlobService blobService,
            UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _cocktailsRepo = cocktailsRepo;
            _config = config;
            _blobService = blobService;
            _cocktailPicturesService = new CocktailPicturesService(blobService);
            _userManager = userManager;
            _productsRepo = productsRepo;
        }

        [HttpGet("Public")]
        public async Task<ActionResult<Helpers.Pagination<CocktailToReturnDto>>> GetCocktails([FromQuery] CocktailSpecParams cocktailParams)
        {
            if (!string.IsNullOrEmpty(cocktailParams.Ingredients))
            {
                cocktailParams.IngredientsList = JsonConvert.DeserializeObject<List<int>>(cocktailParams.Ingredients);
            }

            var spec = new CocktailsWithIngredientsCountSpecification(cocktailParams, false);

            var countSpec = new CocktailsWithFiltersForCountSpecification(cocktailParams, false);

            var totalItems = await _cocktailsRepo.CountAsync(countSpec);

            var cocktailsFromDb = await _cocktailsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<CocktailToReturnDto>>(cocktailsFromDb);

            return Ok(new Helpers.Pagination<CocktailToReturnDto>(cocktailParams.PageIndex,
                cocktailParams.PageSize, totalItems, data));
        }

        [Authorize]
        [HttpGet("Private")]
        public async Task<ActionResult<Helpers.Pagination<CocktailToReturnDto>>> GetPrivateCocktails([FromQuery] CocktailSpecParams cocktailParams)
        {
            if (!string.IsNullOrEmpty(cocktailParams.Ingredients))
            {
                cocktailParams.IngredientsList = JsonConvert.DeserializeObject<List<int>>(cocktailParams.Ingredients);
            }

            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            var spec = new CocktailsWithIngredientsCountSpecification(cocktailParams, true, user.Id);

            var countSpec = new CocktailsWithFiltersForCountSpecification(cocktailParams, true, user.Id);

            var totalItems = await _cocktailsRepo.CountAsync(countSpec);

            var cocktailsFromDb = await _cocktailsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<CocktailToReturnDto>>(cocktailsFromDb);

            return Ok(new Helpers.Pagination<CocktailToReturnDto>(cocktailParams.PageIndex,
                cocktailParams.PageSize, totalItems, data));
        }

        [Authorize]
        [HttpGet("AvailableCocktailsForUser")]
        public async Task<ActionResult<List<CocktailBasicInfoToReturnDto>>> GetCocktailsAvailableForUser()
        {
            var user = await GetAuthorizedUser();

            var spec = new CocktailsForUser(user.Id);

            var cocktails = await _cocktailsRepo.ListAsync(spec);

            var mappedCocktails = _mapper.Map<IReadOnlyList<CocktailBasicInfoToReturnDto>>(cocktails);

            return Ok(mappedCocktails);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CocktailDetailsToReturnDto>> GetCocktailById(int id)
        {
            var spec = new CocktailWithIngredientsSpecification(id);

            var cocktail = await _cocktailsRepo.GetEntityWithSpec(spec);

            if (cocktail == null)
            {
                return NotFound(new Errors.ApiResponse(404));
            }

            if (cocktail.IsPrivate)
            {
                var user = await GetAuthorizedUser();

                if (user == null || cocktail.AuthorId != user.Id)
                {
                    return NotFound(new Errors.ApiResponse(404));
                }
            }

            var cocktailToReturn = _mapper.Map<CocktailDetailsToReturnDto>(cocktail);

            return Ok(cocktailToReturn);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CocktailDetailsToReturnDto>> ManageCocktail([FromForm] CocktailToManageDto dto)
        {
            var user = await GetAuthorizedUser();
            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin");
            bool isEditing = dto.Id != 0;

            var cocktail = CreateCocktailEntityFromDto(dto, user.Id, isAdmin);

            if (isEditing)
            {
                return await UpdateExistingCocktail(cocktail, dto);
            }
            else
            {
                return await CreateNewCocktail(cocktail, dto);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCocktail(int id)
        {
            var user = await GetAuthorizedUser();

            bool isAdmin = await _userManager.IsInRoleAsync(user, "admin");

            var cocktail = await _cocktailsRepo.GetByIdAsync(id);

            if (cocktail == null)
            {
                return NotFound(new Errors.ApiResponse(404));
            }

            if (!cocktail.IsPrivate && !isAdmin)
            {
                return Unauthorized(new Errors.ApiResponse(401));
            }

            await _cocktailsRepo.DeleteAsync(cocktail);

            return NoContent();
        }

        private static void MapEditedCocktailToDbCocktail(Cocktail cocktail, Cocktail cocktailFromDb, bool skipPicture)
        {
            if (!skipPicture)
            {
                cocktailFromDb.Picture = cocktail.Picture;
            }

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

        private async Task<AppUser> GetAuthorizedUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(email);
        }

        private Cocktail CreateCocktailEntityFromDto(CocktailToManageDto dto, string userId, bool isAdmin)
        {
            var cocktail = _mapper.Map<Cocktail>(dto);

            var ingredients = JsonConvert.DeserializeObject<ICollection<IngredientToAddDto>>(dto.Ingredients);
            cocktail.Ingredients = _mapper.Map<ICollection<Ingredient>>(ingredients);
            cocktail.IngredientsCount = cocktail.Ingredients.Count;

            cocktail.AuthorId = userId;

            // Non-admins can only create private cocktails
            if (!isAdmin)
            {
                cocktail.IsPrivate = true;
            }

            return cocktail;
        }

        private async Task<ActionResult<CocktailDetailsToReturnDto>> UpdateExistingCocktail(
            Cocktail cocktailFromDto,
            CocktailToManageDto dto)
        {
            var spec = new CocktailByPrivacyWithIngredientsSpecification(cocktailFromDto.Id, dto.IsPrivate);
            var cocktailFromDb = await _cocktailsRepo.GetEntityWithSpec(spec);

            if (cocktailFromDb == null)
            {
                return NotFound(new Errors.ApiResponse(404));
            }

            bool hasPictureUpdate = await ProcessPictureUpload(dto, cocktailFromDto, cocktailFromDb);

            MapEditedCocktailToDbCocktail(cocktailFromDto, cocktailFromDb, !hasPictureUpdate);
            await _cocktailsRepo.UpdateAsync(cocktailFromDb);

            if (cocktailFromDb.BaseProduct == null)
            {
                cocktailFromDb.BaseProduct = await _productsRepo.GetByIdAsync((int)cocktailFromDb.BaseProductId);
            }

            var editedCocktailToReturn = _mapper.Map<CocktailDetailsToReturnDto>(cocktailFromDb);
            return Ok(editedCocktailToReturn);
        }

        private async Task<ActionResult<CocktailDetailsToReturnDto>> CreateNewCocktail(
            Cocktail cocktail,
            CocktailToManageDto dto)
        {
            cocktail.BaseProductId = GetCocktailBaseProductId(cocktail);
            await _cocktailsRepo.AddAsync(cocktail);

            bool hasPictureUpdate = await ProcessPictureUpload(dto, cocktail, cocktail);

            if (hasPictureUpdate)
            {
                await _cocktailsRepo.UpdateAsync(cocktail);
            }

            var spec = new CocktailByPrivacyWithIngredientsSpecification(cocktail.Id, dto.IsPrivate);
            var cocktailFromDb = await _cocktailsRepo.GetEntityWithSpec(spec);

            var cocktailToReturn = _mapper.Map<CocktailDetailsToReturnDto>(cocktailFromDb);
            return CreatedAtAction(nameof(GetCocktailById), new { id = cocktailToReturn.Id }, cocktailToReturn);
        }

        private async Task<bool> ProcessPictureUpload(
            CocktailToManageDto dto,
            Cocktail cocktailToUpdate,
            Cocktail existingCocktail)
        {
            bool hasPicture = dto.Picture != null && dto.Picture.Length != 0;

            if (!hasPicture)
            {
                return false;
            }

            string container = _config["AzureBlobStorage:PublicCocktailPicturesContainer"];

            await _cocktailPicturesService.DeletePreviousPictureIfExists(existingCocktail, container);

            var newBlobName = _cocktailPicturesService.GenerateNewBlobName(existingCocktail.Id);
            cocktailToUpdate.Picture = $"{container}/{newBlobName}";

            await _blobService.UploadFileStreamBlobAsync(container, dto.Picture.OpenReadStream(), newBlobName);

            return true;
        }
    }
}
