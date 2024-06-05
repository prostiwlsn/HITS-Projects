using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class EditProgramsMessage
    {
        public Guid UserId {  get; set; } 
        public List<ProgramPriorityModel> Programs {  get; set; } = new List<ProgramPriorityModel>();
    }
}
