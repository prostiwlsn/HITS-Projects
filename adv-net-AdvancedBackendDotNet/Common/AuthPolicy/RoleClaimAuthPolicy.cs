using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.AuthPolicy
{
    public class RoleClaimAuthPolicy : IAuthorizationRequirement
    {
        public string Role {  get; set; }
        public RoleClaimAuthPolicy(string role)
        {
            Role = role;
        }
    }
}
