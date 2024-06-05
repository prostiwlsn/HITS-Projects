using Common.Messages;
using Common.Models;
using Common.Result;
using MVCAdmin.Models;

namespace MVCAdmin.Interfaces
{
    public interface IPersonellRequestService
    {
        Task CreateAdminAccount();
        Task<string> GetMyRole(string token);
        Task<string> GetMyId(string token);
        Task<Result<List<PersonellResponseModel>>> GetPersonell(GetPersonellRequest request);
        Task AssignRole(AssignRoleModel model);
        Task AppointToFaculty(AppointToFacultyMessage msg);
        Task AppointManager(AppointManagerMessage msg);
        Task<Result<PersonellResponseModel>> GetManagerInfo(GetManagerInfoRequest request);
    }
}
