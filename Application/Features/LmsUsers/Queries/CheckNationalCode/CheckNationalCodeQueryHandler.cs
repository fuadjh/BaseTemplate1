using Application.Interfaces;

using Common.Wrapper;
using Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LmsUsers.Queries.CheckNationalCode
{
    public class CheckNationalCodeQuery : IRequest<ResponseWrapper<CheckNationalCodeResult>>
    {

        public string NationalCode { get; init; } = default!;
    }

    //================================= Handler  =====================================

    public class CheckNationalCodeQueriesHandler : IRequestHandler<CheckNationalCodeQuery, ResponseWrapper<CheckNationalCodeResult>>
    {
        private readonly IIdentityService _identityService;

        public CheckNationalCodeQueriesHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResponseWrapper<CheckNationalCodeResult>> Handle(CheckNationalCodeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService
                    .CheckNationalCodeAsync(request.NationalCode);

                return ResponseWrapper<CheckNationalCodeResult>.Success(result);
            }
            catch (DomainException ex)
            {
                return ResponseWrapper<CheckNationalCodeResult>.Failed(ex.Message);
            }
        }
    }
}

