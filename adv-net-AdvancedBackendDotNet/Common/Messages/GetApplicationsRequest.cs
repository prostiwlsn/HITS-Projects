using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class GetApplicationsRequest
    {
        public uint size { get; set; }
        public uint page { get; set; }
        public string name { get; set; } = string.Empty;
        public List<string> faculties { get; set; } = new List<string>();
        public ApplicationStatus status { get; set; } = ApplicationStatus.Any;
        public bool? isManagerAppointed { get; set; } = null;
        public Guid managerId { get; set; } = Guid.Empty;
        public bool isDescending { get; set; } = false;
    }
}
