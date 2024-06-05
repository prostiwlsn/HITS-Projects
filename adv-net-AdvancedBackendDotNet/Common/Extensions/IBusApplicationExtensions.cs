using Common.Messages;
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class IBusApplicationExtensions
    {
        public static bool GetApplicationStatus(this IBus bus, Guid id) => bus.Rpc.Request<GetApplicationStatusRequest, bool>(new GetApplicationStatusRequest { Id = id });
    }
}
