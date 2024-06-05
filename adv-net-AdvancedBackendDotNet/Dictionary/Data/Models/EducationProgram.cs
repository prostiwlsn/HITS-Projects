using System.ComponentModel.DataAnnotations;

namespace Dictionary.Data.Models
{
    public class EducationProgram
    {
        [Key]
        public Guid Id { get; set; }
        public string CreateTime { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Language { get; set; }
        public string EducationForm { get; set; }
        public Faculty Faculty { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public bool IsDeleted { get; set; }
    }
}
