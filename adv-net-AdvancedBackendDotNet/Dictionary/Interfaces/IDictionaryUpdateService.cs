using Common.Messages;
using Common.Result;

namespace Dictionary.Interfaces
{
    public interface IDictionaryUpdateService
    {
        Task UpdateEducationLevels();
        Task UpdateDocumentTypes(bool isSecondTime = false);
        Task UpdateFaculties();
        Task UpdatePrograms();

        Task<GetUpdateResponse> GetIsUpdating();
    }
}
