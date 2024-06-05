using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class EditProgramMessage
    {
        public Guid UserId { get; set; }
        public Guid ProgramId { get; set; }
        public uint Priority { get; set; }
    }
}
