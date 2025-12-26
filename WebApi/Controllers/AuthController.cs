using Application.Features.Identity.Command;

using Application.Features.LmsUsers.Command;
using Common.Requests;
using Common.RequestsDto;
using Common.RequestsDto.Users;
using Common.Wrapper;
using Infrastructure.Authorization;
using Infrastructure.IdentityModels;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
     => await Sender.Send(new RegisterUserCommand { registerUserRequest = command })
           is var response && response.IsSuccess
            ? Ok(response)
            : BadRequest(response);


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        => await Sender.Send(new LoginUserCommand { loginRequest = request })
                       is var response && response.IsSuccess
            ? Ok(response)
            : BadRequest(response);


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

        [HttpPost("check-national-code")]
        public async Task<IActionResult> CheckNationalCode( [FromBody] CheckNationalCodeRequest request)
     => await Sender.Send(new CheckNationalCodeCommand { Request = request })
        is var response && response.IsSuccess
            ? Ok(response)
            : BadRequest(response);





    }
}
