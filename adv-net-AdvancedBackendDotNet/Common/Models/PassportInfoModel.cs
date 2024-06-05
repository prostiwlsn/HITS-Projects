using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class PassportInfoModel
    {
        public int SeriesNumber { get; set; }
        public string BirthPlace { get; set; }
        public DateOnly GivenDate { get; set; }
        public string GivenPlace { get; set; }
    }
}
