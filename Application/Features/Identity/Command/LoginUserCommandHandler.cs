using Application.Interfaces;
using Common.RequestsDto;
using Common.ResponsesDto;
using Common.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Command
{
    public class LoginUserCommand : IRequest<ResponseWrapper<AuthenticationResult>>
    {
        public LoginRequest loginRequest { get; set; }
    }

    //================================= Handler  =====================================

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ResponseWrapper<AuthenticationResult>>
    {
        private readonly IIdentityService _identityService;

        public LoginUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResponseWrapper<AuthenticationResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.LoginAsync(request.loginRequest);

            return  ResponseWrapper<AuthenticationResult>.Success(result,"");

        }
    }
}
