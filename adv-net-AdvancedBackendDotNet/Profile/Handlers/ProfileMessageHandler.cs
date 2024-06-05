using Common;
using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Profile.Data;
using Profile.Data.Models;
using Profile.Interfaces;
using Profile.Services;
using System.Runtime.CompilerServices;

namespace Profile.Handlers
{
    public class ProfileMessageHandler
    {
        private ProfileDbContext _dbContext {  get; set; }
        private IProfileService _profileService { get; set; }
        private INotRestrictedDocumentService _documentService { get; set; }

        public ProfileMessageHandler(ProfileDbContext dbContext, IProfileService profileService, INotRestrictedDocumentService documentService)
        {
            _dbContext = dbContext;
            _profileService = profileService;
            _documentService = documentService;
        }

        public async Task<TestRpcResponse> HandleMessage(TestRpcMessage msg)
        {
            return new TestRpcResponse() { Msg = msg.Message };
        }
        public async Task HandleEditProfile(ProfileEditMessage msg)
        {
            try
            {
                await _profileService.EditProfile(msg.ProfileEditModel, msg.UserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task HandleEditPassport(EditPassportMessage msg)
        {
            
            try
            {
                await _documentService.EditPassportInfo(msg.UserId, msg.PassportEditModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task HandleEditEducationDocument(EditEducationalDocumentMessage msg)
        {
            
            try
            {
                await _documentService.EditEducationDocumentInfo(msg.UserId, msg.EducationDocumentEditModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task HandleDeleteFile(DeleteFileMessage msg)
        {
            
            try
            {
                await _documentService.DeleteFile(msg.FileId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task HandleUploadFile(UploadBytesMessage msg)
        {
            
            try
            {
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(msg));
                if (msg.IsPassportFile)
                    await _documentService.UploadPassportFile(msg);
                else
                    await _documentService.UploadEducationDocumentFile(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public async Task<Result> HandleRegistration(RegistrationFormRequest request) => await _profileService.Register(request);
        public async Task<Result<ProfileResponseModel>> HandleGetProfile(GetProfileRequest request) => await _profileService.GetProfile(request.Id);

        public async Task<Result<EducationDocumentInfoModel>> HandleGetEducationDocument(GetEducationDocumentInfoRequest request) => await _documentService.GetEducationDocumentInfo(request.UserId);
        public async Task<Result<PassportInfoModel>> HandleGetPassport(GetPassportInfoRequest request) => await _documentService.GetPassportInfo(request.UserId);

        public async Task<Result<FileList>> HandleGetFilesRequest(GetFilesRequest request)
        {
            if (request.IsPassport)
                return await _documentService.GetPassportFiles(request.UserId);
            else
                return await _documentService.GetEducationDocumentFiles(request.UserId);
        }
        public async Task<Result<GetFileResponse>> GetFile(GetFileRequest request) => await _documentService.GetFile(request.FileId);

        public static ProfileMessageHandler GetRpcHandler(WebApplication app) => app.Services.CreateScope().ServiceProvider.GetRequiredService<ProfileMessageHandler>();
    }
}
