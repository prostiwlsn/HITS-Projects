using Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class EditApplicationModel
    {
        public ProfileEditMessage ProfileEditMessage { get; set; } = new ProfileEditMessage();
        public AppointManagerMessage AppointManagerMessage { get; set; } = new AppointManagerMessage();
        public EditApplicationStatusMessage EditApplicationStatusMessage { get; set; } = new EditApplicationStatusMessage();
    }
}
