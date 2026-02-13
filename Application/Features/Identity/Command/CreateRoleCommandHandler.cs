using Application.Interfaces;
using Common.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Command
{
    public record CreateRoleCommand(string RoleName) : IRequest<ResponseWrapper<string>>;

    //================================= Handler  =====================================
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ResponseWrapper<string>>
    {
        private readonly IIdentityService _identityService;

        public CreateRoleCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResponseWrapper<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.CreateRoleAsync(request.RoleName);
        }
    }

}
