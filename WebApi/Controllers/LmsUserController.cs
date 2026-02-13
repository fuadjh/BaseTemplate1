using Application.Features.LmsUsers.Queries.CheckNationalCode;
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
        public async Task<IActionResult> CheckNationalCode([FromBody] CheckNationalCodeQuery request)
 => await Sender.Send(new CheckNationalCodeQuery { NationalCode = request.NationalCode })
    is var response && response.IsSuccess
        ? Ok(response)
        : BadRequest(response);




     



    }
}
