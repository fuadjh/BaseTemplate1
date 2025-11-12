using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly ISender Sender;

        public BaseApiController(ISender sender)
        {
            Sender = sender;
        }
    }
}
