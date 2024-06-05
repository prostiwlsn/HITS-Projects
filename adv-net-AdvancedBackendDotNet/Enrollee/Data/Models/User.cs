using System.ComponentModel.DataAnnotations;

namespace Auth.Data.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
