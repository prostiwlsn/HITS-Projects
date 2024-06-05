using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class EditApplicationStatusMessage
    {
        public Guid UserId { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}
