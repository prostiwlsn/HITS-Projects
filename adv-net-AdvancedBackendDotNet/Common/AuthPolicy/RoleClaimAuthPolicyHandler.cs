using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.AuthPolicy
{
    public class RoleClaimAuthPolicyHandler : AuthorizationHandler<RoleClaimAuthPolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleClaimAuthPolicy requirement)
        {
            // Получаем текущего пользователя
            var user = context.User;

            // Получаем все claims пользователя
            var claims = user.Claims;

            // Получаем конкретный claim по типу
            var userRoleClaim = user.FindFirst(ClaimTypes.Role);

            // Проверяем наличие определенного claim
            if (user.HasClaim(ClaimTypes.Role, "Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //var param = context.Requirements.

            if (user.HasClaim(ClaimTypes.Role, "MainManager"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}