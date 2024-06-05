using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profile.Data.Models
{
    [PrimaryKey(nameof(Id), nameof(UserId))]
    public abstract class Document
    {
        public Guid Id { get; set; }
        [ForeignKey(name: "UserId")]
        public Guid UserId { get; set; }
        //public ProfileInformation Profile { get; set; }
        public List<DocumentFile> Files { get; set; } = new List<DocumentFile>();
    }
}
