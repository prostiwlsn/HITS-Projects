using System.ComponentModel.DataAnnotations;

namespace Dictionary.Data.Models
{
    public class Faculty
    {
        [Key]
        public Guid Id { get; set; }
        public string CreateTime { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public List<EducationProgram> EducationPrograms { get; set; }
    }
}
