using API.Errors;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
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

            if (roleExists) return BadRequest(new ApiResponse(400, "Such role already exists"));

            var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));

            return Ok(roleResult);
        }

        [HttpPost("AddUserToRole")]
        public async Task<ActionResult> AddUserToRole(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return BadRequest(new ApiResponse(400));

            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok();
        }
    }
}
