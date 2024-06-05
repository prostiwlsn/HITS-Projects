using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class EducationDocumentInfoModel
    {
        public string Name { get; set; }
        public Guid DocumentTypeId { get; set; }
        public int[] NextEducationLevels { get; set; }
    }
}
