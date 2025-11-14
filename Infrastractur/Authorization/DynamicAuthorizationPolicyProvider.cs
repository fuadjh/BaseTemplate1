using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authorization
{
    public class DynamicAuthorizationPolicyProvider
    : DefaultAuthorizationPolicyProvider
    {
        public DynamicAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options) { }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // اگر Policy قبلاً تعریف شده بود → همان را برگردان
            var policy = await base.GetPolicyAsync(policyName);
            if (policy != null)
                return policy;

            // ایجاد Policy داینامیک
            var dynamicPolicy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();

            return dynamicPolicy;
        }
    }
}
