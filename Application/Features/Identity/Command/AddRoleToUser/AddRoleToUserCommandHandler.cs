using Application.Interfaces;
using Common.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Command.AddRoleToUser
{

    public record AddRoleToUserCommand(string Email, string RoleName) : IRequest<ResponseWrapper<string>>;

    //================================= Handler  =====================================
    public class AddRoleToUserCommandHandler : IRequestHandler<AddRoleToUserCommand, ResponseWrapper<string>>
    {
        private readonly IIdentityService _identityService;

        public AddRoleToUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResponseWrapper<string>> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.AddRoleToUserAsync(request.Email, request.RoleName);
        }
    }
}
