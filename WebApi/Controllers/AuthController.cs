using Common.Requests;
using Common.RequestsDto;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized("User not found");

            var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!validPassword)
                return Unauthorized("Invalid credentials");

            var token = await _jwtTokenService.GenerateTokenAsync(user);

            return Ok(new
            {
                Token = token,
                User = new { user.Id, user.UserName, user.Email }
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            // بررسی وجود کاربر
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest("User with this email already exists.");

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            // می‌توانی اینجا نقش (Role) هم اضافه کنی در صورت نیاز
            // await _userManager.AddToRoleAsync(user, "User");

            // تولید توکن پس از ثبت‌نام
            var token = _jwtTokenService.GenerateTokenAsync(user);
            return Ok(new { Token = token });
        }

    }
}
