using Application.Features.Identity.Command;
using Common.Requests;
using Common.RequestsDto;
using Common.Wrapper;
using Infrastructure.Identity;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
       private readonly ISender _sender; // از MediatR برای ارسال کامند

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseWrapper<string>().Failed("Invalid request data."));

            var response = await _sender.Send(command);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequest request)
        //{
        //    var user = await _userManager.FindByEmailAsync(request.Email);
        //    if (user == null)
        //        return Unauthorized("User not found");

        //    var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        //    if (!validPassword)
        //        return Unauthorized("Invalid credentials");

        //    var token = await _jwtTokenService.GenerateTokenAsync(user);

        //    return Ok(new
        //    {
        //        Token = token,
        //        User = new { user.Id, user.UserName, user.Email }
        //    });
        //}



    }
}
