using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Application.Data.Models
{
    [PrimaryKey(nameof(ApplicationInfoId), nameof(ProgramId))]
    public class ChosenProgram
    {
        public Guid ApplicationInfoId { get; set; }
        public ApplicationInfo Application { get; set; }

        public Guid ProgramId { get; set; }
        public string ProgramName { get; set; }

        public Guid FacultyId { get; set; }
        public string FacultyName { get; set; }

        //[1:5]
        public uint Priority { get; set; }
    }
}
