using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class UploadFileMessage
    {
        public Guid UserId { get; set; }
        public bool IsPassportFile { get; set; }
        public FileModel FileModel { get; set; } = new FileModel();
    }
}
