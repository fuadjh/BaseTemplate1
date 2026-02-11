using Application.Interfaces;
using Common.RequestsDto.Users;
using Common.ResponsesDto;
using Common.ResponsesDto.LmsUsers;
using Common.ResponsesDto.Users;
using Common.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LmsUsers.Command
{
    public class GetLmsUsersQuery : IRequest<ResponseWrapper<PagedResult<LmsUserDto>>>
    {
        public GetLmsUsersQuery Request { get; set; } = default!;
    }
    public class GetLmsUsersQueryHandler  : IRequestHandler<GetLmsUsersQuery, ResponseWrapper<PagedResult<LmsUserDto>>>
    {
        private readonly IIdentityService _identityService;

        public GetLmsUsersQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResponseWrapper<PagedResult<LmsUserDto>>> Handle(
            GetLmsUsersQuery query,
            CancellationToken cancellationToken)
        {
            var result = _identityService.CheckNationalCodeAsync(query.Request);
            return await ResponseWrapper<PagedResult<result>>.Success();
        }
    }


}
