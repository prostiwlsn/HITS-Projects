using Common.Models;
using System.ComponentModel.DataAnnotations;

namespace MVCAdmin.Models
{
    public class AssignRoleModel
    {
        [EmailAddress]
        public string Email { get; set; }
        public Roles Role { get; set; }
    }
}
