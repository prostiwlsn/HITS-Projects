using Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Application.Data.Models
{
    public class ApplicationInfo
    {
        [Key]
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

        public Guid? ManagerId { get; set; } = null;
        public string ManagerName { get; set; } = string.Empty;
        public string ManagerEmail { get; set; } = string.Empty;
        public List<ChosenProgram> ChosenPrograms { get; set; }

        public DateTime LastChange {  get; set; }
    }
}
