using Application.Features.Identity.Command;
using Common.Requests;
using Common.RequestsDto;
using Common.Wrapper;
using Infrastructure.Identity;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        public AuthController(ISender sender) : base(sender)
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest command)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseWrapper<string>().Failed("Invalid request data."));

            var response = await Sender.Send(new RegisterUserCommand {registerUserRequest =command });

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseWrapper<string>().Failed("Invalid request data."));

            var response = await Sender.Send(new LoginUserCommand { loginRequest=request});

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
        {
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRoleToUser([FromBody] AddRoleToUserCommand command)
        {
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }




    }
}
