using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class UploadBytesMessage
    {
        public Guid UserId { get; set; }
        public bool IsPassportFile { get; set; }
        public byte[] Bytes { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}
