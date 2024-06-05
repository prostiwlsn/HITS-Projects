using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using MVCAdmin.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace MVCAdmin.Services
{
    public class ProfileRequestService : IProfileRequestService
    {
        private HttpClient _httpClient;
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;
        public ProfileRequestService(HttpClient httpClient, IConfiguration configuration, IBus bus)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _bus = bus;
        }

        public async Task<Result> EditProfile(string token, ProfileEditModel model)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, _configuration.GetSection("URls:Profile:Host").Value + _configuration.GetSection("URls:Profile:Profile").Value))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using (JsonContent content = JsonContent.Create(model))
                {
                    request.Content = content;
                    HttpResponseMessage response = await _httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        return new Result { Success = true, Message = "Ok" };
                    }
                    else
                    {
                        return new Result { Success = false };
                    }
                }
            }
        }

        public async Task<Result<ProfileResponseModel>> GetProfile(string token)
        {
            Console.WriteLine(token);
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _configuration.GetSection("URls:Profile:Host").Value + _configuration.GetSection("URls:Profile:Profile").Value))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(request));
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    ProfileResponseModel model = JsonConvert.DeserializeObject<ProfileResponseModel>(jsonString);
                    return new Result<ProfileResponseModel> { Success = true, Message = "Ok", Load = model };
                }
                else
                {
                    return new Result<ProfileResponseModel> { Success = false };
                }
            }
        }

        public async Task<Result<ProfileResponseModel>> GetApplicantProfile(GetProfileRequest request)
        {
            var response = await _bus.Rpc.RequestAsync<GetProfileRequest, Result<ProfileResponseModel>>(request);
            return response;
        }
        public async Task<Result> EditApplicantProfile(ProfileEditMessage msg)
        {
            await _bus.PubSub.PublishAsync(msg);
            return new Result { Success = true };
        }

        public async Task<Result<PassportInfoModel>> GetPassportInfo(GetPassportInfoRequest request)
        {
            var response = await _bus.Rpc.RequestAsync<GetPassportInfoRequest, Result<PassportInfoModel>>(request);
            return response;
        }
        public async Task<Result> EditPassportInfo(EditPassportMessage msg)
        {
            await _bus.PubSub.PublishAsync(msg);
            return new Result { Success = true };
        }

        public async Task<Result<EducationDocumentInfoModel>> GetEducationDocumentInfo(GetEducationDocumentInfoRequest request)
        {
            var response = await _bus.Rpc.RequestAsync<GetEducationDocumentInfoRequest, Result<EducationDocumentInfoModel>>(request);
            return response;
        }
        public async Task<Result> EditEducationDocumentInfo(EditEducationalDocumentMessage msg)
        {
            await _bus.PubSub.PublishAsync(msg);
            return new Result { Success = true };
        }

        public async Task<Result<FileList>> GetFiles(GetFilesRequest request)
        {
            var response = await _bus.Rpc.RequestAsync<GetFilesRequest, Result<FileList>>(request);
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response.Load));
            return response;
        }
        public async Task<Result<GetFileResponse>> GetFile(GetFileRequest request)
        {
            var response = await _bus.Rpc.RequestAsync<GetFileRequest, Result<GetFileResponse>>(request);
            return response;
        }
        public async Task<Result> DeleteFile(DeleteFileMessage msg)
        {
            await _bus.PubSub.PublishAsync(msg);
            return new Result { Success = true };
        }
        public async Task<Result> UploadFile(UploadFileMessage msg)
        {
            //Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(msg));
            //await _bus.PubSub.PublishAsync(msg);

            byte[] bytes = await ConvertIFormFileToByteArray(msg.FileModel.File);
            UploadBytesMessage newMsg = new UploadBytesMessage { UserId = msg.UserId, IsPassportFile = msg.IsPassportFile, Bytes = bytes, FileExtension = ".pdf"};
            await _bus.PubSub.PublishAsync(newMsg);

            return new Result { Success = true };
        }

        private async Task<byte[]> ConvertIFormFileToByteArray(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
