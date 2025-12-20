using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ILmsUserRepository
    {
        Task<LmsUser?> GetByIdentityUserIdAsync(int identityUserId);
        Task AddAsync(LmsUser user);
    }
}
