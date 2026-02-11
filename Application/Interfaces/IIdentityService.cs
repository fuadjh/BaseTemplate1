using Application.Features.LmsUsers.Queries.CheckNationalCode;
using Common.RequestsDto;

using Common.ResponsesDto;

using Common.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IIdentityService
    {

        Task<CheckNationalCodeResult> CheckNationalCodeAsync(string request);
        Task<ResponseWrapper<string>> RegisterAsync(RegisterUserRequest registerUserCommand);
        Task<AuthenticationResult> LoginAsync(LoginRequest loginRequest);
        Task<ResponseWrapper<string>> AddRoleToUserAsync(string email, string roleName);
        Task<ResponseWrapper<string>> CreateRoleAsync(string roleName);
        Task<Guid> GetOrCreateUserByNationalCodeAsync(string nationalCode);
    }
}

