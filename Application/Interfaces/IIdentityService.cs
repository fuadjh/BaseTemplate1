using Common.RequestsDto;
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
        Task<ResponseWrapper<string>> RegisterAsync(RegisterUserRequest registerUserCommand);
        Task<ResponseWrapper<string>> LoginAsync(LoginRequest loginRequest);
        Task<ResponseWrapper<string>> AddRoleToUserAsync(string email, string roleName);
        Task<ResponseWrapper<string>> CreateRoleAsync(string roleName);
    }
}

