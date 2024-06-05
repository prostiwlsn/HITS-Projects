using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ChosenProgramInfoModel
    {
        public Guid ProgramId { get; set; }
        public string ProgramName { get; set; }

        public Guid FacultyId { get; set; }
        public string FacultyName { get; set; }

        //[1:5]
        public uint Priority { get; set; }
    }
}
