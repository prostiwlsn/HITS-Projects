using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ProgramInfoModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Language { get; set; }
        public string EducationForm { get; set; }

        public Guid FacultyId { get; set; }
        public string FacultyName {  get; set; }
        public int EducationLevelId { get; set; }
    }
}
