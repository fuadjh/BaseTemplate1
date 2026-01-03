using Common.RequestsDto;
using Common.RequestsDto.Users;
using Common.ResponsesDto;
using Common.ResponsesDto.Users;
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

        Task<CheckNationalCodeResult> CheckNationalCodeAsync(CheckNationalCodeRequest request);
        Task<ResponseWrapper<string>> RegisterAsync(RegisterUserRequest registerUserCommand);
        Task<AuthenticationResult> LoginAsync(LoginRequest loginRequest);
        Task<ResponseWrapper<string>> AddRoleToUserAsync(string email, string roleName);
        Task<ResponseWrapper<string>> CreateRoleAsync(string roleName);
        Task<Guid> GetOrCreateUserByNationalCodeAsync(string nationalCode);
    }
}

