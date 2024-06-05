using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class AssignRoleMessage
    {
        public Guid UserId {  get; set; }
        public Roles Role { get; set; }
    }
}
