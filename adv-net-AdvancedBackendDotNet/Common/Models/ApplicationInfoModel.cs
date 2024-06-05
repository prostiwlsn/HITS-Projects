using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ApplicationInfoModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Created;
        public bool IsClosed { get; set; }

        public string EducationDocumentName { get; set; } = string.Empty;
        public Guid EducationDocumentId { get; set; }
        public int[] NextEducationLevels { get; set; } = new int[0];

        public Guid? ManagerId { get; set; }
        public string ManagerName { get; set; } = string.Empty;
        public List<Guid> Faculties { get; set; } = new List<Guid>();

        public DateTime LastChange { get; set; }
    }
}
