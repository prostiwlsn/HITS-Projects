using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class AppointToFacultyMessage
    {
        public Guid ManagerId { get; set; }
        public string FacultyId { get; set; } = Guid.Empty.ToString();
    }
}
