using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class PersonellResponseModel
    {
        public string? Email {  get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? SecondName { get; set; }
        public Roles Role { get; set; }

        public Guid Id { get; set; }

        public string? FacultyName { get; set; }
        public Guid? FacultyId { get; set; }
    }
}
