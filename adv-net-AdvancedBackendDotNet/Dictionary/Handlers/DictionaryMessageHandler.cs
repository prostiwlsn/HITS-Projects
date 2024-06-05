using Common.Messages;
using Common.Models;
using Common.Result;
using Dictionary.Interfaces;
using EasyNetQ.Events;

namespace Dictionary.Handlers
{
    public class DictionaryMessageHandler
    {
        private IDictionaryUpdateService _updateService;
        private IDictionaryService _service;
        public DictionaryMessageHandler(IDictionaryUpdateService updateService, IDictionaryService dictionaryService)
        {
            _updateService = updateService;
            _service = dictionaryService;
        }

        public async Task HandleUpdate(UpdateMessage msg)
        {
            try
            {

                switch (msg.Type)
                {
                    case (DictionaryInfoType.EducationLevels):
                        await _updateService.UpdateEducationLevels();
                        break;
                    case (DictionaryInfoType.DocumentTypes):
                        await _updateService.UpdateDocumentTypes();
                        break;
                    case (DictionaryInfoType.Faculties):
                        await _updateService.UpdateFaculties();
                        break;
                    case (DictionaryInfoType.Programs):
                        await _updateService.UpdatePrograms();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<Result<DocumentTypeInfoModel>> HandleGetDocumentTypeInfo(GetDocumentTypeInfoRequest request) => _service.GetDocumentTypeInfo(request.Id);

        public async Task<Result<EducationLevelInfoModel>> HandleGetEducationLevelInfo(GetEducationLevelInfoRequest request) => _service.GetEducationLevelInfo(request.Id);

        public async Task<Result<FacultyInfoModel>> HandleGetFacultyInfo(GetFacultyInfoRequest request) => _service.GetFacultyInfo(request.Id);

        public async Task<Result<ProgramInfoModel>> HandleGetProgramInfo(GetProgramInfoRequest request) => _service.GetProgramInfo(request.Id);

        public async Task<GetUpdateResponse> GetUpdate() => await _updateService.GetIsUpdating();

        public static DictionaryMessageHandler GetHandler(WebApplication app) => 
            app.Services.CreateScope().ServiceProvider.GetRequiredService<DictionaryMessageHandler>();
    }
}
