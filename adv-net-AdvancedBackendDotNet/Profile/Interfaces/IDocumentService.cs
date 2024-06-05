using Common.Models;
using Common.Result;
//using Profile.Models;

//вынести перегрузки методов в другой интерфейс

namespace Profile.Interfaces
{
    public interface IDocumentService
    {
        Task<Result> EditPassportInfo(Guid userId, PassportEditModel model);
        Task<Result> EditEducationDocumentInfo(Guid userId, EducationDocumentEditModel model);

        Task<Result<PassportInfoModel>> GetPassportInfo(Guid userId);
        Task<Result<EducationDocumentInfoModel>> GetEducationDocumentInfo(Guid userId);

        Task<Result<FileList>> GetPassportFiles(Guid userId);
        Task<Result<FileList>> GetEducationDocumentFiles(Guid userId);

        Task<Result<string>> GetFile(Guid fileId, Guid userId);
        
        Task<Result> DeleteFile(Guid fileId, Guid userId);
        
        Task<Result> UploadPassportFile(Guid userId, FileModel model);
        Task<Result> UploadEducationDocumentFile(Guid userId, FileModel model);
    }
}
