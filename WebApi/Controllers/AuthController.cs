using Application.Features.Identity.Command;
using Application.Features.Identity.Command.AddRoleToUser;
using Application.Features.LmsUsers.Command;
using Common.RequestsDto;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Requests;
using WebApi.Controllers;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        public AuthController(ISender sender ,IMapper mapper) : base(sender, mapper)
        {
        }
//-----------------------------------------------------------------
        [HttpPost("register_user")]
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


       // [HttpPost("create-role")]
       // public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommandHandler command)
       // {
        //    var result = await Sender.Send(command);
      //      return result.IsSuccess ? Ok(result) : BadRequest(result);
       // }

        [HttpPost("add-role")]

        public async Task<IActionResult> AddRoleToUser([FromBody] AddRoleToUserCommand command)
        {
            var result = await Sender.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpPost("register_user")]
        public async Task<IActionResult> Register([FromBody] RegisterLmsUserRequest request)
     => await Sender.Send(Mapper.خطا <RegisterLmsUserCommand>(request)) is var response
        && response.IsSuccess
         ? Ok(response)
         : BadRequest(response);


    }




}

