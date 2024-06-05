using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    //document.Id, document.EducationLevel, document.Name, document.CreateTime, document.NextEducationLevels
    public class DocumentTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EducationLevelInfoModel EducationLevel { get; set; }
        public List<EducationLevelInfoModel> NextEducationLevels { get; set; }
        public string CreateTime { get; set; }
    }
}
