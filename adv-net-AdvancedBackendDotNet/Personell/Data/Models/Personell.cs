using Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Personell.Data.Models
{
    public abstract class Personell
    {
        [Key]
        public Guid UserId { get; set; }
        public Guid? AppointerId { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? SecondName { get; set; }

        public Roles Roles { get; set; }
    }
}
