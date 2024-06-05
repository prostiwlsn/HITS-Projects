using Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class ProfileEditModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SecondName { get; set; }
        public Gender Gender { get; set; }
        public string Citizenship { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
