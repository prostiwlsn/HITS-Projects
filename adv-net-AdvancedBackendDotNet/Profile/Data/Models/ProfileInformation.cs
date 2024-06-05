using Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Profile.Data.Models
{
    public class ProfileInformation
    {
        [Key]
        public Guid UserId { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string SecondName { get; set; }

        public Gender Gender { get; set; }
        public string Citizenship { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public Passport Passport { get; set; } 
        public EducationDocument EducationDocument { get; set; }
    }
}
