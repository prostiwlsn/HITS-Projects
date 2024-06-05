using System.ComponentModel.DataAnnotations;

namespace Dictionary.Data.Models
{
    public class UpdateState
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsFinished { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
