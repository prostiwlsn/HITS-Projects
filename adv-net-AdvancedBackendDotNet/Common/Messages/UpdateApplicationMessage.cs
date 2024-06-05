using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class UpdateApplicationMessage
    {
        public Guid Id { get; set; }
        public bool IsEditedByApplicant { get; set; } = false;
    }
}
