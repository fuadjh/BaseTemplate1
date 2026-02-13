
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly ISender Sender;
        private readonly IMapper Mapper;
        public BaseApiController(ISender sender, IMapper mapper)
        {
            Sender = sender;
            Mapper = mapper;
    }
    }
}
