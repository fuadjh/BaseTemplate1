using Application.Interfaces;
using Common.RequestsDto.Users;
using Common.ResponsesDto.Users;
using Common.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.LmsUsers
{
    public class CheckNationalCodeCommand: IRequest<ResponseWrapper<CheckNationalCodeResult>>
    {
        public CheckNationalCodeRequest Request { get; set; } = default!;
    }
    public class CheckNationalCodeCommandHandler  : IRequestHandler<CheckNationalCodeCommand, ResponseWrapper<CheckNationalCodeResult>>
    {
        private readonly IIdentityService _identityService;

        public CheckNationalCodeCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResponseWrapper<CheckNationalCodeResult>> Handle(
            CheckNationalCodeCommand request,
            CancellationToken cancellationToken)
        {
            return await _identityService.CheckNationalCodeAsync(request.Request);
        }
    }


}
