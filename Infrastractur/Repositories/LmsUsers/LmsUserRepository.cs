using Application.Interfaces.Repositories;
using Domain.Users;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
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

        public async Task<LmsUser?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await _context.LmsUsers
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<LmsUser?> GetByIdentityUserIdAsync(
            Guid identityUserId,
            CancellationToken cancellationToken)
        {
            return await _context.LmsUsers
                .FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId, cancellationToken);
        }

        public async Task AddAsync(
            LmsUser user,
            CancellationToken cancellationToken)
        {
            await _context.LmsUsers.AddAsync(user, cancellationToken);
        }

        public Task UpdateAsync(LmsUser user, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }


}
