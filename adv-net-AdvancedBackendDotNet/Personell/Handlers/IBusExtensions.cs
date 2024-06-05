using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;

namespace Personell.Handlers
{
    public static class IBusExtensions
    {
        public static async Task SetupListeners(this IBus bus, WebApplication app)
        {
            //bus.Rpc.Respond<TestRpcMessage, TestRpcResponse>(request => MessageHandler.GetRpcHandler(app).HandleMessage(request));
            //bus.PubSub.Subscribe<TestMessage>("profile_subscription_id", msg => Console.WriteLine(msg.Message));

            await bus.PubSub.SubscribeAsync<AssignRoleMessage>("assign_role_id", async msg => await PersonellMessageHandler.GetHandler(app).HandleAssignUserMessage(msg));
            await bus.PubSub.SubscribeAsync<AppointToFacultyMessage>("appoint_manager_id", async msg => await PersonellMessageHandler.GetHandler(app).HandleAppointToFacultyMessage(msg));
            await bus.PubSub.SubscribeAsync<UpdatePersonell>("update_personell_id", async msg => await PersonellMessageHandler.GetHandler(app).HandleUpdatePersonell(msg));

            //bus.Rpc.Respond<RegistrationFormRequest, Result>(request => PersonellMessageHandler.GetRpcHandler(app).HandleRegistration(request));

            await bus.Rpc.RespondAsync<GetRoleRequest, Result<Roles>>(async request => await PersonellMessageHandler.GetHandler(app).HandleGetRoleRequest(request));
            await bus.Rpc.RespondAsync<GetPersonellRequest, Result<List<PersonellResponseModel>>>(async request => await PersonellMessageHandler.GetHandler(app).HandleGetPersonell(request));
            await bus.Rpc.RespondAsync<AppointManagerRequest, Result<PersonellResponseModel>>(async request => await PersonellMessageHandler.GetHandler(app).HandleAppointToApplicationRequest(request));
            await bus.Rpc.RespondAsync<GetManagerInfoRequest, Result<PersonellResponseModel>>(async request => await PersonellMessageHandler.GetHandler(app).HandleGetManager(request));
        }
    }
}