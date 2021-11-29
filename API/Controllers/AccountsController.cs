using API.Dtos.Accounts;
using API.Errors;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
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
        public async Task<ActionResult<UserDetailsDto>> GetUserDetails()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            var userToReturn = _mapper.Map<UserDetailsDto>(user);

            return userToReturn;
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet("SendForgottenPasswordLink")]
        public async Task<ActionResult> SendForgottenPasswordLink([FromQuery]string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return Ok();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);

            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);

            var link = $"{_config["ClientUrl"]}/ChangeForgottenPassword?token={tokenEncoded}&email={user.Email}";

            await _emailService.SendForgottenPasswordLink(_config, user, link);

            return Ok();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserLoginReturnDto>> Login(UserLoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401, "Dane logowania są nieprawidłowe."));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);

            if (!result.Succeeded)
            {
                bool isEmailConfimed = await _userManager.IsEmailConfirmedAsync(user);

                if (!isEmailConfimed) return Unauthorized(new ApiResponse(401, "Adres email nie został potwierdzony"));

                return Unauthorized(new ApiResponse(401, "Dane logowania są nieprawidłowe."));
            }

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

        [HttpPost("Register")]
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

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);

            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);

            var confirmationLink = $"{_config["ClientUrl"]}/confirmemail?token={tokenEncoded}&email={user.Email}";

            await _emailService.SendConfirmationEmailForRegisteredUser(_config, user, confirmationLink);

            return new UserRegisterReturnDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName
            };
        }

        [HttpPatch("ChangeForgottenPassword")]
        public async Task<ActionResult<bool>> ChangeForgottenPassword(ChangeForgottenPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null) return BadRequest(new ApiResponse(400, "Token wygasł lub adres email jest niepoprawny"));

            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(dto.Token);

            var tokenDecoded = Encoding.UTF8.GetString(tokenDecodedBytes);

            var result = await _userManager.ResetPasswordAsync(user, tokenDecoded, dto.NewPassword);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Token wygasł lub adres email jest niepoprawny"));

            return Ok();
        }

        [HttpPatch("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(ConfirmEmailDto confirmEmail)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmail.Email);

            if (user == null) return BadRequest(new ApiResponse(401));

            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(confirmEmail.Token);

            var tokenDecoded = Encoding.UTF8.GetString(tokenDecodedBytes);

            var result = await _userManager.ConfirmEmailAsync(user, tokenDecoded);

            if (!result.Succeeded) return BadRequest(new ApiResponse(401));

            return NoContent();
        }

        [Authorize]
        [HttpPatch("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto passwordDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "Wprowadzone hasła są niepoprawne"));

            return NoContent();
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

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return NoContent();
        }
    }
}
