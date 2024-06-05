using Common.Messages;
using MVCAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCAdmin.Models
{
    public class EditPersonellModel
    {
        public ProfileEditMessage ProfileEditMessage { get; set; } = new ProfileEditMessage();
        public AppointToFacultyMessage AppointToFacultyMessage { get; set; } = new AppointToFacultyMessage();
        public AssignRoleModel AssignRoleMessage { get; set; } = new AssignRoleModel();
    }
}
