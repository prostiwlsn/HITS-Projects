using Common.Models;

namespace Personell.Data.Models
{
    public class Manager : Personell
    {
        public Guid? FacultyId { get; set; }
        public string? FacultyName { get; set; }

        public List<Application> Applications { get; set; }

        //public override Roles Roles { get; set; } = Roles.Manager;
    }
}
