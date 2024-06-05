using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class EditPassportMessage
    {
        public Guid UserId { get; set; }
        public PassportEditModel PassportEditModel { get; set; } = new PassportEditModel();
    }
}
