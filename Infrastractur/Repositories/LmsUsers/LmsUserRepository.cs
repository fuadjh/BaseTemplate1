using Application.Interfaces.Repositories;
using Domain.Users;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.LmsUsers
{
    public class LmsUserRepository : ILmsUserRepository
    {
        private readonly ApplicationDbContext _context;

        public LmsUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LmsUser?> GetByIdAsync(Guid id, CancellationToken ct)
            => await _context.LmsUsers.FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<LmsUser?> GetByIdentityUserIdAsync(int identityUserId, CancellationToken ct)
            => await _context.LmsUsers
                .FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId, ct);

        public async Task AddAsync(LmsUser user, CancellationToken ct)
            => await _context.LmsUsers.AddAsync(user, ct);
    }

}
