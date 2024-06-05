using System.ComponentModel.DataAnnotations;

namespace Auth.Data.Models
{
    public class Session
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime Expires { get; set; }
    }
}
