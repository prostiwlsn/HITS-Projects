using Common.Messages;
using Common.Result;

namespace Profile.Interfaces
{
    public interface INotRestrictedDocumentService : IDocumentService
    {
        Task<Result<GetFileResponse>> GetFile(Guid fileId);
        Task<Result> DeleteFile(Guid fileId);
        Task<Result> UploadPassportFile(UploadBytesMessage msg);
        Task<Result> UploadEducationDocumentFile(UploadBytesMessage msg);
    }
}
