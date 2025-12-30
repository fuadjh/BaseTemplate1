using Application.Features.Identity.Command;
using Application.Features.LmsUsers.Command;
using Common.RequestsDto;
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

        [HttpGet("get-lms-users")]
        public async Task<IActionResult> GetLmsUsers(int pageNumber = 1,   int pageSize = 10)
     => await Sender.Send(new GetLmsUsersQuery(pageNumber, pageSize))
           is var response && response.IsSuccess
            ? Ok(response)
            : BadRequest(response);
    }
}
