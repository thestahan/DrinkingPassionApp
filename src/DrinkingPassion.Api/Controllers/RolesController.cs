using DrinkingPassion.Api.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrinkingPassion.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<IdentityRole>>> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return Ok(roles);
        }

        [HttpPost]
        public async Task<ActionResult<IdentityRole>> AddRole(string name)
        {
            var roleExists = await _roleManager.RoleExistsAsync(name);

            if (roleExists)
            {
                return BadRequest(new DrinkingPassion.Api.Errors.ApiResponse(400, "Such role already exists"));
            }

            var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));

            return Ok(roleResult);
        }

        [HttpPost("AddUserToRole")]
        public async Task<ActionResult> AddUserToRole(DrinkingPassion.Api.Dtos.Roles.AddUserToRoleDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.UserEmail);

            if (user == null)
            {
                return BadRequest(new DrinkingPassion.Api.Errors.ApiResponse(400, "User with atemptted email wasn't found"));
            }

            var roleExists = await _roleManager.RoleExistsAsync(dto.Role);

            if (!roleExists)
            {
                return BadRequest(new DrinkingPassion.Api.Errors.ApiResponse(400, "Attempted role doesn't exist"));
            }

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok();
        }
    }
}
