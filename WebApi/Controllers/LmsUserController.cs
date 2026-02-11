using Application.Features.Identity.Command;
using Application.Features.LmsUsers.Command;
using Application.Features.LmsUsers.Queries.CheckNationalCode;
using Common.RequestsDto;
using Common.RequestsDto.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class LmsUserController : BaseApiController
    {
        public LmsUserController(ISender sender) : base(sender)
        {
        }

        [HttpPost("check-national-code")]
        public async Task<IActionResult> CheckNationalCode([FromBody] string request)
 => await Sender.Send(new CheckNationalCodeQuery { Request = request })
    is var response && response.IsSuccess
        ? Ok(response)
        : BadRequest(response);




     



    }
}
