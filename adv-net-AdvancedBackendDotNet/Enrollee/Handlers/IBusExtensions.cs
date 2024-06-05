using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;

namespace Auth.Handlers
{
    public static class IBusExtensions
    {
        public static void SetupListeners(this IBus bus, WebApplication app)
        {
            bus.Rpc.Respond<GetUserIdRequest, Result<UserIdResponseModel>>(request => AuthMessageHandler.GetRpcHandler(app).HandleGetUserId(request));
        }
    }
}
