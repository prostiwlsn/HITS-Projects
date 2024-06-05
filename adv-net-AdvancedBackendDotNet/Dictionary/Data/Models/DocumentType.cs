using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary.Data.Models
{
    public class DocumentType
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CreateTime { get; set; }
        [ForeignKey("EducationLevelId")]
        public int EducationLevelId { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public List<EducationLevel> NextEducationLevels { get; set; }
        public bool IsDeleted { get; set; }
    }
}
