using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class PassportEditModel
    {
        [Required]
        public int SeriesNumber { get; set; }
        [Required]
        public string BirthPlace { get; set; }
        [Required]
        public DateTime GivenDate { get; set; }
        [Required]
        public string GivenPlace { get; set; }
    }
}
