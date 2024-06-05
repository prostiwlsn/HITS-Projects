using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class DocumentTypeInfoModel
    {
        public string Name { get; set; }
        public int EducationLevelId { get; set; }
        public List<int> NextEducationLevels { get; set; }
    }
}
