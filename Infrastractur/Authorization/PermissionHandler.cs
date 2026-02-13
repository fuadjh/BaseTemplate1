using Infrastructure.Context;
using Infrastructure.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceProvider _provider;

        public PermissionHandler(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return;

            using var scope = _provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var intUserId = Guid.Parse(userId);

            var hasPermission = await (
                from userRole in db.UserRoles
                join role in db.Roles on userRole.RoleId equals role.Id
                join rp in db.RolePermissions on role.Id equals rp.RoleId
                join p in db.Permissions on rp.PermissionId equals p.Id
                where userRole.UserId == intUserId &&
                      p.Name == requirement.PermissionName
                select p
            ).AnyAsync();

            if (hasPermission)
                context.Succeed(requirement);
        }
    }
}
