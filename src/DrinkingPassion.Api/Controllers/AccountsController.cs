﻿using AutoMapper;
using DrinkingPassion.Api.Core.Entities.Identity;
using DrinkingPassion.Api.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DrinkingPassion.Api.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IMapper mapper, ITokenService tokenService, IEmailService emailService, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _emailService = emailService;
            _config = config;
        }

        [Authorize]
        [HttpGet("Details")]
        public async Task<ActionResult<Dtos.Accounts.UserDetailsToReturnDto>> GetUserDetails()
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;

            var user = await _userManager.FindByEmailAsync(email);

            var userToReturn = _mapper.Map<Dtos.Accounts.UserDetailsToReturnDto>(user);

            return userToReturn;
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet("SendForgottenPasswordLink")]
        public async Task<ActionResult> SendForgottenPasswordLink([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Ok();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);

            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);

            var link = $"{_config["ClientUrl"]}/ChangeForgottenPassword?token={tokenEncoded}&email={user.Email}";

            await _emailService.SendForgottenPasswordLink(_config, user, link);

            return Ok();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Dtos.Accounts.UserLoginReturnDto>> Login(DrinkingPassion.Shared.Models.Users.UserLoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return Unauthorized(new Errors.ApiErrorResponse(401, "Dane logowania są nieprawidłowe."));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);

            if (!result.Succeeded)
            {
                bool isEmailConfimed = await _userManager.IsEmailConfirmedAsync(user);

                if (!isEmailConfimed)
                {
                    return Unauthorized(new Errors.ApiErrorResponse(401, "Adres email nie został potwierdzony"));
                }

                return Unauthorized(new Errors.ApiErrorResponse(401, "Dane logowania są nieprawidłowe."));
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var jwt = _tokenService.CreateToken(user, userRoles);

            return new Dtos.Accounts.UserLoginReturnDto
            {
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Token = jwt,
                TokenExpiration = "86400",
                Roles = userRoles
            };
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Dtos.Accounts.UserRegisterReturnDto>> Register(Dtos.Accounts.UserRegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new Errors.ApiValidationErrorResponse
                {
                    Errors = new[] { "Email address is in use" }
                });
            }

            var user = _mapper.Map<AppUser>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new Errors.ApiErrorResponse(400));
            }

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);

            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);

            var confirmationLink = $"{_config["ClientUrl"]}/confirmemail?token={tokenEncoded}&email={user.Email}";

            await _emailService.SendConfirmationEmailForRegisteredUser(_config, user, confirmationLink);

            return new Dtos.Accounts.UserRegisterReturnDto
            {
                Email = user.Email!,
                DisplayName = user.DisplayName
            };
        }

        [HttpPatch("ChangeForgottenPassword")]
        public async Task<ActionResult<bool>> ChangeForgottenPassword(Dtos.Accounts.UserChangeForgottenPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return BadRequest(new Errors.ApiErrorResponse(400, "Token wygasł lub adres email jest niepoprawny"));
            }

            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(dto.Token);

            var tokenDecoded = Encoding.UTF8.GetString(tokenDecodedBytes);

            var result = await _userManager.ResetPasswordAsync(user, tokenDecoded, dto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new Errors.ApiErrorResponse(400, "Token wygasł lub adres email jest niepoprawny"));
            }

            return Ok();
        }

        [HttpPatch("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(Dtos.Accounts.UserConfirmEmailDto confirmEmail)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmail.Email);

            if (user == null)
            {
                return BadRequest(new Errors.ApiErrorResponse(401));
            }

            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(confirmEmail.Token);

            var tokenDecoded = Encoding.UTF8.GetString(tokenDecodedBytes);

            var result = await _userManager.ConfirmEmailAsync(user, tokenDecoded);

            if (!result.Succeeded)
            {
                return BadRequest(new Errors.ApiErrorResponse(401));
            }

            return NoContent();
        }

        [Authorize]
        [HttpPatch("ChangePassword")]
        public async Task<ActionResult> ChangePassword(Dtos.Accounts.UserChangePasswordDto passwordDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return BadRequest(new Errors.ApiErrorResponse(400));
            }

            var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new Errors.ApiErrorResponse(400, "Wprowadzone hasła są niepoprawne"));
            }

            return NoContent();
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateUserProfile(Dtos.Accounts.UserUpdateDto updateDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email)!;

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return BadRequest(new Errors.ApiErrorResponse(400));
            }

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.BartenderType = updateDto.BartenderType;
            user.DisplayName = updateDto.DisplayName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new Errors.ApiErrorResponse(400));
            }

            return NoContent();
        }
    }
}
