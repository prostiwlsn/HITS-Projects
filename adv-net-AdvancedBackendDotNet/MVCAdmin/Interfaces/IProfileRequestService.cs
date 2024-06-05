using Common.Messages;
using Common.Models;
using Common.Result;

namespace MVCAdmin.Interfaces
{
    public interface IProfileRequestService
    {
        Task<Result<ProfileResponseModel>> GetProfile(string token);
        Task<Result> EditProfile(string token, ProfileEditModel model);
        Task<Result<ProfileResponseModel>> GetApplicantProfile(GetProfileRequest request);
        Task<Result> EditApplicantProfile(ProfileEditMessage msg);

        Task<Result<PassportInfoModel>> GetPassportInfo(GetPassportInfoRequest request);
        Task<Result> EditPassportInfo(EditPassportMessage msg);
        Task<Result<EducationDocumentInfoModel>> GetEducationDocumentInfo(GetEducationDocumentInfoRequest request);
        Task<Result> EditEducationDocumentInfo(EditEducationalDocumentMessage msg);
        Task<Result<FileList>> GetFiles(GetFilesRequest request);
        Task<Result<GetFileResponse>> GetFile(GetFileRequest request);
        Task<Result> DeleteFile(DeleteFileMessage msg);
        Task<Result> UploadFile(UploadFileMessage msg);
    }
}
