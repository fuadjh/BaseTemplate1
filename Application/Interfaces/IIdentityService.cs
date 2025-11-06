using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool Succeeded, string? UserId, string? Error)> RegisterAsync(string email, string password, string fullName);
        Task<string?> LoginAsync(string email, string password);
    }
}

