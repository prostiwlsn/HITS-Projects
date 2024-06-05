using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Microsoft.VisualBasic;

namespace Application.Handlers
{
    public static class IBusExtensions
    {
        public static void SetupListeners(this IBus bus, WebApplication app)
        {
            bus.PubSub.Subscribe<EditProgramsMessage>("programs_edit_id", async msg => await ApplicationMessageHandler.GetHandler(app).HandleEditPrograms(msg));
            bus.PubSub.Subscribe<EditProgramMessage>("program_edit_id", async msg => await ApplicationMessageHandler.GetHandler(app).HandleEditProgram(msg));
            bus.PubSub.Subscribe<DeleteProgramMessage>("program_delete_id", async msg => await ApplicationMessageHandler.GetHandler(app).HandleDeleteProgram(msg));
            bus.PubSub.Subscribe<DeleteProgramsMessage>("programs_delete_id", async msg => await ApplicationMessageHandler.GetHandler(app).HandleDeletePrograms(msg));
            bus.Rpc.Respond<GetProgramsMessage, Result<List<ChosenProgramInfoModel>>>(async request => await ApplicationMessageHandler.GetHandler(app).HandleGetPrograms(request));

            bus.PubSub.Subscribe<UpdateApplicationMessage>("application_update_id", async msg => await ApplicationMessageHandler.GetHandler(app).HandleApplicationUpdateMessage(msg));
            bus.PubSub.Subscribe<EditApplicationStatusMessage>("application_status_id", async msg => await ApplicationMessageHandler.GetHandler(app).HandleEditApplicationStatusMessage(msg));
            bus.PubSub.Subscribe<AppointManagerMessage>("application_manager_id", async msg => await ApplicationMessageHandler.GetHandler(app).HandleAppointManager(msg));

            bus.Rpc.Respond<GetApplicationsRequest, Result<List<ApplicationInfoModel>>>(async request => await ApplicationMessageHandler.GetHandler(app).HadleGetApplicationsRequest(request));
            bus.Rpc.Respond<GetApplicationRequest, Result<ApplicationInfoModel>>(async request => await ApplicationMessageHandler.GetHandler(app).HandleGetApplicationRequest(request));

            bus.Rpc.Respond<GetApplicationStatusRequest, bool>(async request => await ApplicationMessageHandler.GetHandler(app).HandleGetStatus(request));
        }
    }
}
