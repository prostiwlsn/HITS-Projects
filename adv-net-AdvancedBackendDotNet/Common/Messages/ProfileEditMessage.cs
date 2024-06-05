using Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class ProfileEditMessage
    {
        public Guid UserId { get; set; }
        public ProfileEditModel ProfileEditModel { get; set; } = new ProfileEditModel();
    }
}
