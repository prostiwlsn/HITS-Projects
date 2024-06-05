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
    public class DictionaryRequestService : IDictionaryRequestService
    {
        private IBus _bus;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public DictionaryRequestService(IBus bus, HttpClient httpClient, IConfiguration configuration)
        {
            _bus = bus;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task UpdateDocumentTypes()
        {
            await _bus.PubSub.PublishAsync<UpdateMessage>(new UpdateMessage { Type = DictionaryInfoType.DocumentTypes });
        }

        public async Task UpdateEducationLevels()
        {
            await _bus.PubSub.PublishAsync<UpdateMessage>(new UpdateMessage { Type = DictionaryInfoType.EducationLevels });
        }

        public async Task UpdateFaculties()
        {
            await _bus.PubSub.PublishAsync<UpdateMessage>(new UpdateMessage { Type = DictionaryInfoType.Faculties });
        }

        public async Task UpdatePrograms()
        {
            await _bus.PubSub.PublishAsync<UpdateMessage>(new UpdateMessage { Type = DictionaryInfoType.Programs });
        }

        public async Task<bool> GetUpdateState()
        {
            var response = await _bus.Rpc.RequestAsync<GetUpdateRequest, GetUpdateResponse>(new GetUpdateRequest());
            return response.IsStarted;
        }

        public async Task<List<FacultyInfoModel>> GetFaculties()
        {
            using (HttpResponseMessage response = await _httpClient.GetAsync(_configuration.GetSection("URls:Dictionary:Host").Value + _configuration.GetSection("URls:Dictionary:Faculties").Value))
            {
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    List<FacultyInfoModel> list = JsonConvert.DeserializeObject<List<FacultyInfoModel>>(jsonString) ?? new List<FacultyInfoModel>();
                    return list;
                }
                else
                {
                    return new List<FacultyInfoModel>();
                }
            }
        }

        public async Task<List<DocumentTypeDto>> GetDocumentTypes()
        {
            using (HttpResponseMessage response = await _httpClient.GetAsync(_configuration.GetSection("URls:Dictionary:Host").Value + _configuration.GetSection("URls:Dictionary:DocumentTypes").Value))
            {
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    List<DocumentTypeDto> list = JsonConvert.DeserializeObject<List<DocumentTypeDto>>(jsonString)??new List<DocumentTypeDto>();
                    return list;
                }
                else
                {
                    return new List<DocumentTypeDto>();
                }
            }
        }
    }
}
