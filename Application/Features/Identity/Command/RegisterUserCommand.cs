using Application.Interfaces;
using Common.RequestsDto;
using Common.ResponsesDto;
using Common.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Command
{
    public class RegisterUserCommand: IRequest<ResponseWrapper<string>>
    {
        public RegisterUserRequest registerUserRequest { get; set; }
    }

  

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseWrapper<string>>
    {
        private readonly IIdentityService _identityService;

        public RegisterUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResponseWrapper<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RegisterAsync(request.registerUserRequest.email, request.registerUserRequest.password, request.registerUserRequest.fullName);

            if (result.Succeeded)
                return new ResponseWrapper<string>().Success(result.UserId, "User registered successfully");

            return new ResponseWrapper<string>().Failed(result.Error ?? "User registration failed");
        }
    }


}
