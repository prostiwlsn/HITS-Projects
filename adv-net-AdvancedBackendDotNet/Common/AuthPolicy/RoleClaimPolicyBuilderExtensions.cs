using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.AuthPolicy
{
    public static class RoleClaimPolicyBuilderExtensions
    {
        public static void AddAdminOnlyPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options => options.AddPolicy("AdminOnly", policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Admin");
            }));
        }

        public static void AddMainManagerOnlyPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options => options.AddPolicy("MainManagerOnly", policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Admin", "MainManager");
            }));
        }

        public static void AddManagerOnlyPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options => options.AddPolicy("ManagerOnly", policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "Admin", "MainManager", "Manager");
            }));
        }
    }
}

/*
services.AddAuthorization(options =>
{
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.RequireClaim("ClaimType1", "Value1");
        policy.RequireClaim("ClaimType2", "Value2");
    });
});
*/