using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class CredentialChangeTokenModel
    {
        [Required]
        public string ChangeToken { get; set; }
        [MinLength(8)]
        [MaxLength(25)]
        [Required]
        public string NewValue { get; set; }
    }
}
