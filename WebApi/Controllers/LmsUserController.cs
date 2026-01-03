using Application.Features.Identity.Command;
using Application.Features.LmsUsers.Command;
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
        public async Task<IActionResult> CheckNationalCode([FromBody] CheckNationalCodeRequest request)
 => await Sender.Send(new CheckNationalCodeCommand { Request = request })
    is var response && response.IsSuccess
        ? Ok(response)
        : BadRequest(response);




     



    }
}
