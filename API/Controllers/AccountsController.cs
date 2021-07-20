using API.Dtos.Accounts;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpGet("details")]
        public async Task<ActionResult<UserDetailsDto>> GetUserDetails()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            var userToReturn = _mapper.Map<UserDetailsDto>(user);

            return userToReturn;
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginReturnDto>> Login(UserLoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            var userRoles = await _userManager.GetRolesAsync(user);

            var jwt = _tokenService.CreateToken(user, userRoles);

            return new UserLoginReturnDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = jwt,
                TokenExpiration = "86400",
                Roles = userRoles
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserRegisterReturnDto>> Register(UserRegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse { 
                    Errors = new [] { "Email address is in use" } 
                });
            }

            var user = _mapper.Map<AppUser>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            await _userManager.AddToRoleAsync(user, "User");

            return new UserRegisterReturnDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName
            };
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateUserProfile(UserUpdateDto updateDto)
        {
            //var userToUpdate = _mapper.Map<AppUser>(updateDto);

            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.BartenderType = updateDto.BartenderType;
            user.DisplayName = updateDto.DisplayName;

            var result = await _userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}
