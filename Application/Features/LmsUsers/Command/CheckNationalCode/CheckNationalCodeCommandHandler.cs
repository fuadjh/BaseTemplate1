using Application.Interfaces;
using Common.ResponsesDto.Users;
using Common.Wrapper;
using Domain.Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LmsUsers.Command.CheckNationalCode
{
    public class CheckNationalCodeCommand : IRequestHandler<CheckNationalCodeCommand, ResponseWrapper<CheckNationalCodeResult>>
    {
        private readonly IIdentityService _identityService;

        public CheckNationalCodeCommand(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResponseWrapper<CheckNationalCodeResult>> Handle(CheckNationalCodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService
                    .CheckNationalCodeAsync(request.Request);

                return ResponseWrapper<CheckNationalCodeResult>.Success(result);
            }
            catch (DomainException ex)
            {
                return ResponseWrapper<CheckNationalCodeResult>.Failed(ex.Message);
            }
        }
    }
}
