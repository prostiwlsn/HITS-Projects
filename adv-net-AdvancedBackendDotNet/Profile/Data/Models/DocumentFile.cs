using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Data.Models
{
    public class DocumentFile
    {
        [Key]
        public Guid FileId { get; set; }
        public Document Document { get; set; }
        public string Path { get; set; }
    }
}