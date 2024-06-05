using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class DeleteProgramsMessage
    {
        public List<Guid> Ids { get; set; }
    }
}
