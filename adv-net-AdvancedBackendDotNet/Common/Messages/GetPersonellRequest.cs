using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class GetPersonellRequest
    {
        public uint size { get; set; } = 10;
        public uint page { get; set; } = 1;
        public string email { get; set; } = string.Empty;
    }
}
