using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authorization
{
    public class CustomPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomPolicyProvider(IOptions<AuthorizationOptions> options, IServiceProvider serviceProvider)
            : base(options)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // خواندن پرمیشن از دیتابیس
            var exists = await db.Permissions.AnyAsync(x => x.Name == policyName);

            if (!exists)
                return await base.GetPolicyAsync(policyName);

            return new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();
        }
    }
}
