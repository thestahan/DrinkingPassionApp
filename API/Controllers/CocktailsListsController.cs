using API.Dtos.Cocktails;
using API.Dtos.CocktailsLists;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        public CocktailsListsController(IGenericRepository<CocktailsList> listsRepo, UserManager<AppUser> userManager, IMapper mapper, IGenericRepository<Cocktail> cocktailsRepo)
        {
            _listsRepo = listsRepo;
            _userManager = userManager;
            _mapper = mapper;
            _cocktailsRepo = cocktailsRepo;
        }

        [HttpGet]
        public async Task<ActionResult> GetCocktailsLists()
        {
            var user = await GetAuthorizedUser();

            var spec = new CocktailsListsForUser(user.Id);

            var cocktailsLists = await _listsRepo.ListAsync(spec);

            var mappedLists = _mapper.Map<List<CocktailsListDto>>(cocktailsLists);

            return Ok(mappedLists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCocktailsListDetails(int id)
        {
            var user = await GetAuthorizedUser();

            var spec = new CocktailsListForUserByIdWithCocktails(user.Id, id);

            var list = await _listsRepo.GetEntityWithSpec(spec);

            if (list == null) return NotFound(new ApiResponse(404));

            var mappedList = _mapper.Map<CocktailsListDetailsDto>(list);

            return Ok(mappedList);
        }

        [AllowAnonymous]
        [HttpGet("{id}/guestview")]
        public async Task<ActionResult> GetCocktailsListForGuest(int id)
        {
            var spec = new CocktailsListByIdWithCocktails(id);

            var list = await _listsRepo.GetEntityWithSpec(spec);

            if (list == null) return NotFound(new ApiResponse(404));

            var mappedList = _mapper.Map<CocktailsListDetailsDto>(list);

            return Ok(mappedList);
        }

        [AllowAnonymous]
        [HttpGet("{id}/guestview/{cocktailId}")]
        public async Task<ActionResult> GetCocktailFromList(int id, int cocktailId)
        {
            var spec = new CocktailsListWithCocktailExists(id, cocktailId);

            var listWithCoctailExists = await _listsRepo.EntityExistsWithSpecAsync(spec);

            if (!listWithCoctailExists) return NotFound(new ApiResponse(404));

            var cocktailSpec = new CocktailWithIngredientsSpecification(id);

            var cocktail = await _cocktailsRepo.GetEntityWithSpec(cocktailSpec);

            if (cocktail == null) return NotFound(new ApiResponse(404));

            var mappedCocktail = _mapper.Map<CocktailDetailsToReturnDto>(cocktail);

            return Ok(mappedCocktail);
        }

        private async Task<AppUser> GetAuthorizedUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email)) return null;

            return await _userManager.FindByEmailAsync(email);
        }
    }
}
