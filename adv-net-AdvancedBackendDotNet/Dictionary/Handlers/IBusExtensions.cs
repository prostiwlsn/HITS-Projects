using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;

namespace Dictionary.Handlers
{
    public static class IBusExtensions
    {
        public static void SetupListeners(this IBus bus, WebApplication app)
        {
            bus.PubSub.Subscribe<UpdateMessage>("update_dictionary_id", msg => DictionaryMessageHandler.GetHandler(app).HandleUpdate(msg));

            bus.Rpc.Respond<GetUpdateRequest, GetUpdateResponse>(request => DictionaryMessageHandler.GetHandler(app).GetUpdate());

            bus.Rpc.RespondAsync<GetDocumentTypeInfoRequest, Result<DocumentTypeInfoModel>>(async request => await DictionaryMessageHandler.GetHandler(app).HandleGetDocumentTypeInfo(request));

            bus.Rpc.RespondAsync<GetEducationLevelInfoRequest, Result<EducationLevelInfoModel>>(async request => await DictionaryMessageHandler.GetHandler(app).HandleGetEducationLevelInfo(request));

            bus.Rpc.RespondAsync<GetFacultyInfoRequest, Result<FacultyInfoModel>>(async request => await DictionaryMessageHandler.GetHandler(app).HandleGetFacultyInfo(request));

            bus.Rpc.RespondAsync<GetProgramInfoRequest, Result<ProgramInfoModel>>(async request => await DictionaryMessageHandler.GetHandler(app).HandleGetProgramInfo(request));
        }
    }
}
