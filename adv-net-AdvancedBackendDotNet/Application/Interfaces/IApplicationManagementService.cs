using Common.Messages;
using Common.Models;
using Common.Result;

namespace Application.Interfaces
{
    public interface IApplicationManagementService
    {
        Task<Result> EditApplicationStatus(Guid id, ApplicationStatus status);
        Task<Result<List<ApplicationInfoModel>>> GetApplications(uint size, uint page, string name, List<Guid> faculties, 
            ApplicationStatus status, bool? isManagerAppointed, 
            Guid managerId, bool? isDescending);
        Task<Result<ApplicationInfoModel>> GetApplication(Guid userId);
        Task<Result> AppointManager(AppointManagerMessage msg);
    }
}
