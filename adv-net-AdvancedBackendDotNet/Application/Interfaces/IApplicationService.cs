using Common.Result;

namespace Application.Interfaces
{
    public interface IApplicationService
    {
        Task<Result> CreateApplication(Guid id);
        Task<Result> DeleteApplication(Guid id);
        Task<Result> EditApplication(Guid id);
        Task<Result> UpdateApplication(Guid id, bool isEditedByApplicant = false);
        Task<bool> GetApplicationStatus(Guid id);
    }
}
