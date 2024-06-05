using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class EmailChangeModel
    {
        [EmailAddress]
        [Required]
        public string NewEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
