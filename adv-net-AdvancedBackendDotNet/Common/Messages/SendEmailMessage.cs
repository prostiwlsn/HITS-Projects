﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messages
{
    public class SendEmailMessage
    {
        public string To { get; set; }
        public string Message { get; set; }
        public string Topic { get; set; } = "noreply";
    }
}
