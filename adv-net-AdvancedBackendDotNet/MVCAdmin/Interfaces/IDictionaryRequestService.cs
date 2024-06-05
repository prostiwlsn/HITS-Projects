using Common.Models;

namespace MVCAdmin.Interfaces
{
    public interface IDictionaryRequestService
    {
        Task UpdateDocumentTypes();
        Task UpdateEducationLevels();
        Task UpdateFaculties();
        Task UpdatePrograms();

        Task<bool> GetUpdateState();

        Task<List<FacultyInfoModel>> GetFaculties();
        Task<List<DocumentTypeDto>> GetDocumentTypes();
    }
}
