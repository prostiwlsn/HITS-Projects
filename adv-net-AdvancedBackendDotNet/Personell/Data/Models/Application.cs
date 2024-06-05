using System.ComponentModel.DataAnnotations;

namespace Personell.Data.Models
{
    public class Application
    {
        [Key]
        public Guid ApplicationId { get; set; }

        //applicant information
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;

        public Manager Manager { get; set; }
    }
}
