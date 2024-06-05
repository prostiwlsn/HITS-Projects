using Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class ProfileResponseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string SecondName { get; set; }
        public Gender Gender { get; set; }
        public string Citizenship { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
