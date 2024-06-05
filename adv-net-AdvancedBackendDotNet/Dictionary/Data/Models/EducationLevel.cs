using System.ComponentModel.DataAnnotations;

namespace Dictionary.Data.Models
{
    public class EducationLevel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public List<DocumentType> DocumentTypes { get; set; }
    }
}
