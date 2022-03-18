using API.Dtos.Cocktails;
using API.Dtos.CocktailsLists;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Extensions;
using Core.Interfaces;
using Core.Specifications.Cocktails;
using Core.Specifications.CocktailsLists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class CocktailsListsController : BaseApiController
    {
        private readonly IGenericRepository<CocktailsList> _listsRepo;
        private readonly IGenericRepository<Cocktail> _cocktailsRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public CocktailsListsController(
            IGenericRepository<CocktailsList> listsRepo,
            UserManager<AppUser> userManager,
            IMapper mapper,
            IGenericRepository<Cocktail> cocktailsRepo)
        {
            _listsRepo = listsRepo;
            _userManager = userManager;
            _mapper = mapper;
            _cocktailsRepo = cocktailsRepo;
        }

        [HttpGet]
        public async Task<ActionResult<CocktailsListDto>> GetCocktailsLists()
        {
            var user = await GetAuthorizedUser();

            var spec = new CocktailsListsForUser(user.Id);

            var cocktailsLists = await _listsRepo.ListAsync(spec);

            var mappedLists = _mapper.Map<List<CocktailsListDto>>(cocktailsLists);

            return Ok(mappedLists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CocktailsListDetailsDto>> GetCocktailsListDetails(int id)
        {
            var user = await GetAuthorizedUser();

            var spec = new CocktailsListForUserByIdWithCocktails(user.Id, id);

            var list = await _listsRepo.GetEntityWithSpec(spec);

            if (list == null) return NotFound(new ApiResponse(404));

            var mappedList = _mapper.Map<CocktailsListDetailsDto>(list);

            return Ok(mappedList);
        }

        [AllowAnonymous]
        [HttpGet("{username}/{listSlug}")]
        public async Task<ActionResult<CocktailsListDetailsDto>> GetCocktailsListForGuest(string userName, string listSlug)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return NotFound(new ApiResponse(404));

            var spec = new CocktailsListByUserIdAndSlug(userName, listSlug);

            var list = await _listsRepo.GetEntityWithSpec(spec);

            if (list == null) return NotFound(new ApiResponse(404));

            var mappedList = _mapper.Map<CocktailsListDetailsDto>(list);

            return Ok(mappedList);
        }

        [AllowAnonymous]
        [HttpGet("{username}/{listSlug}/{cocktailId}")]
        public async Task<ActionResult<CocktailDetailsToReturnDto>> GetCocktailFromList(string userName, string listSlug, int cocktailId)
        {
            var spec = new CocktailsListWithCocktailExists(userName, listSlug, cocktailId);

            var listWithCoctailExists = await _listsRepo.EntityExistsWithSpecAsync(spec);

            if (!listWithCoctailExists) return NotFound(new ApiResponse(404));

            var cocktailSpec = new CocktailWithIngredientsSpecification(cocktailId);

            var cocktail = await _cocktailsRepo.GetEntityWithSpec(cocktailSpec);

            if (cocktail == null) return NotFound(new ApiResponse(404));

            var mappedCocktail = _mapper.Map<CocktailDetailsToReturnDto>(cocktail);

            return Ok(mappedCocktail);
        }

        [HttpPost]
        public async Task<ActionResult<CocktailsListDetailsDto>> ManageCocktailsList(CocktailsListToAddDto dto)
        {
            var user = await GetAuthorizedUser();

            if (dto.Cocktails.Count == 0) return BadRequest(new ApiResponse(400));

            var list = _mapper.Map<CocktailsList>(dto);

            var spec = new CocktailsPublicAndPrivateByIds(dto.Cocktails, user.Id);

            var cocktails = await _cocktailsRepo.ListAsync(spec);

            if (cocktails.Count != dto.Cocktails.Count) return BadRequest(new ApiResponse(400));

            if (dto.Id != 0)
            {
                var listSpec = new CocktailsListByIdWithCocktails(dto.Id);

                var listFromDb = await _listsRepo.GetEntityWithSpec(listSpec);

                if (listFromDb.AuthorId != user.Id) return BadRequest(new ApiResponse(400));

                listFromDb.Cocktails = cocktails.ToList();
                listFromDb.CocktailsCount = cocktails.Count;
                listFromDb.Name = dto.Name;
                listFromDb.Slug = dto.Name.Slugify();

                await _listsRepo.UpdateAsync(listFromDb);

                return Ok(_mapper.Map<CocktailsListDetailsDto>(listFromDb));
            }

            list.Cocktails = cocktails.ToList();
            list.AuthorId = user.Id;
            list.Slug = dto.Name.Slugify();

            var addedList = await _listsRepo.AddAsync(list);

            var listToReturn = _mapper.Map<CocktailsListDetailsDto>(addedList);

            return CreatedAtAction(nameof(GetCocktailsListDetails), new { id = listToReturn.Id }, listToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCocktailsList(int id)
        {
            var spec = new CocktailsListForUserById(id);

            var list = await _listsRepo.GetEntityWithSpec(spec);

            if (list == null) return NotFound(new ApiResponse(404));

            await _listsRepo.DeleteAsync(list);

            return NoContent();
        }

        private async Task<AppUser> GetAuthorizedUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email)) return null;

            return await _userManager.FindByEmailAsync(email);
        }
    }
}
