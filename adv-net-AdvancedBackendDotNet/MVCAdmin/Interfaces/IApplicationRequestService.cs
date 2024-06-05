using Common.Messages;
using Common.Models;
using Common.Result;

namespace MVCAdmin.Interfaces
{
    public interface IApplicationRequestService
    {
        Task<Result<List<ApplicationInfoModel>>> GetApplications(GetApplicationsRequest request);
        Task EditApplicationStatus(EditApplicationStatusMessage msg);

        Task<Result<ApplicationInfoModel>> GetApplication(GetApplicationRequest request);
        Task EditApplication(EditApplicationModel msg);
        Task<Result<List<ChosenProgramInfoModel>>> GetPrograms(GetProgramsMessage msg);
        Task<Result> EditPrograms(EditProgramsMessage msg);
        Task<Result> EditProgram(EditProgramMessage msg);
        Task DeleteProgram(DeleteProgramMessage msg);
    }
}
