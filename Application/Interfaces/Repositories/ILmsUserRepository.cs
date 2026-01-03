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
        Task<LmsUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<LmsUser?> GetByIdentityUserIdAsync(Guid identityUserId, CancellationToken cancellationToken);

        Task AddAsync(LmsUser user, CancellationToken cancellationToken);
        
        Task UpdateAsync(LmsUser user, CancellationToken ct);
    }


}
