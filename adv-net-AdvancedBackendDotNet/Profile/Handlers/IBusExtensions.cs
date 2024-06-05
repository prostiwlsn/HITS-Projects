using Common;
using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using EasyNetQ.Management.Client.Model;

namespace Profile.Handlers
{
    public static class IBusExtensions
    {
        public static void SetupListeners(this IBus bus, WebApplication app)
        {
            //bus.Rpc.Respond<TestRpcMessage, TestRpcResponse>(request => MessageHandler.GetRpcHandler(app).HandleMessage(request));
            //bus.PubSub.Subscribe<TestMessage>("profile_subscription_id", msg => Console.WriteLine(msg.Message));
            bus.PubSub.Subscribe<ProfileEditMessage>("profile_edit_id", async msg => await ProfileMessageHandler.GetRpcHandler(app).HandleEditProfile(msg));
            bus.PubSub.Subscribe<EditPassportMessage>("passport_edit_id", async msg => await ProfileMessageHandler.GetRpcHandler(app).HandleEditPassport(msg));
            bus.PubSub.Subscribe<EditEducationalDocumentMessage>("ed_edit_id", async msg => await ProfileMessageHandler.GetRpcHandler(app).HandleEditEducationDocument(msg));

            bus.PubSub.Subscribe<DeleteFileMessage>("file_delete_id", async msg => await ProfileMessageHandler.GetRpcHandler(app).HandleDeleteFile(msg));
            bus.PubSub.Subscribe<UploadBytesMessage>("file_upload_id", async msg => await ProfileMessageHandler.GetRpcHandler(app).HandleUploadFile(msg));

            bus.Rpc.Respond<RegistrationFormRequest, Result>(async request => await ProfileMessageHandler.GetRpcHandler(app).HandleRegistration(request));
            bus.Rpc.Respond<GetProfileRequest, Result<ProfileResponseModel>>(async request => await ProfileMessageHandler.GetRpcHandler(app).HandleGetProfile(request));

            bus.Rpc.Respond<GetEducationDocumentInfoRequest, Result<EducationDocumentInfoModel>>(async request => await ProfileMessageHandler.GetRpcHandler(app).HandleGetEducationDocument(request));
            bus.Rpc.Respond<GetPassportInfoRequest, Result<PassportInfoModel>>(async request => await ProfileMessageHandler.GetRpcHandler(app).HandleGetPassport(request));

            bus.Rpc.Respond<GetFilesRequest, Result<FileList>>(async request => await ProfileMessageHandler.GetRpcHandler(app).HandleGetFilesRequest(request));
            bus.Rpc.Respond<GetFileRequest, Result<GetFileResponse>>(async request => await ProfileMessageHandler.GetRpcHandler(app).GetFile(request));
        }
    }
}
